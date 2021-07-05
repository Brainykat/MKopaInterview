using Accounts.Domain.Entities;
using Common.Base.Shared.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Data.Seed
{
  public class AccountsContextSeed
  {
    public async Task SeedAsync(AccountsContext context, ILogger<AccountsContextSeed> logger, int retries = 3)
    {
      var policy = CreatePolicy(retries, logger, nameof(AccountsContextSeed));

      await policy.ExecuteAsync(async () =>
      {
        if (!context.ChartOfAccounts.Any())
        {
          await context.ChartOfAccounts.AddRangeAsync(
              GetPreconfiguredClients());
          await context.SaveChangesAsync();
        }
      });
    }

    private List<ChartOfAccount> GetPreconfiguredClients()
    {
      return new List<ChartOfAccount>
      {
        //Office Chart
        ChartOfAccount.Create(1010,"Office Bank Account", AccountType.Revenues, Statement.Income_Statement),
        ChartOfAccount.Create(1011, "Office MNO Account", AccountType.Revenues, Statement.Income_Statement),
        ChartOfAccount.Create(1012, "Office Cash Account", AccountType.Revenues, Statement.Income_Statement),
        //Client Chart
        ChartOfAccount.Create(2010,"Customer Account Receivable", AccountType.Revenues, Statement.Balance_Sheet),

        //Loan Chart
        ChartOfAccount.Create(3010,"Customer Solar Loan Account", AccountType.Revenues, Statement.Balance_Sheet),
        ChartOfAccount.Create(3011,"Customer Fridge Loan Account", AccountType.Revenues, Statement.Balance_Sheet),
        //......
        //Other products
      };
    }

    private AsyncRetryPolicy CreatePolicy(int retries, ILogger<AccountsContextSeed> logger, string prefix)
    {
      return Policy.Handle<SqlException>().
          WaitAndRetryAsync(
              retryCount: retries,
              sleepDurationProvider: retry => TimeSpan.FromSeconds(5),
              onRetry: (exception, timeSpan, retry, ctx) =>
              {
                logger.LogWarning(exception, "[{prefix}] Exception {ExceptionType} with message {Message} detected on attempt {retry} of {retries}", prefix, exception.GetType().Name, exception.Message, retry, retries);
              }
          );
    }
  }
}
