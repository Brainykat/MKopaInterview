using Accounts.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Data.Repositories
{
  

  public class ChartOfAccountRepository : IChartOfAccountRepository
  {
    private readonly AccountsContext context;
    public ChartOfAccountRepository(AccountsContext context)
    {
      this.context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<List<ChartOfAccount>> GetChartOfAccounts() =>
        await context.ChartOfAccounts.ToListAsync();
    public async Task<List<ChartOfAccount>> GetOfficeCOAs() =>
        await context.ChartOfAccounts.Where(c => c.Number > 1000 && c.Number < 1999).ToListAsync();
    public async Task<List<ChartOfAccount>> GetCustomerCOAs() =>
        await context.ChartOfAccounts.Where(c => c.Number > 2000 && c.Number < 2999).ToListAsync();

    public async Task<ChartOfAccount> GetChartOfAccount(int number) =>
        await context.ChartOfAccounts.Include(y => y.Accounts).FirstOrDefaultAsync(c => c.Number == number);
    public async Task<ChartOfAccount> FindChartOfAccount(string name) =>
        await context.ChartOfAccounts.Include(y => y.Accounts).FirstOrDefaultAsync(c => c.Description.Contains(name));
    public async Task<ChartOfAccount> GetChartOfAccount(Guid id) =>
        await context.ChartOfAccounts.Include(y => y.Accounts).FirstOrDefaultAsync(c => c.Id == id);

    public async Task Add(ChartOfAccount chart)
    {
      context.ChartOfAccounts.Add(chart);
      await context.SaveChangesAsync();
    }
  }
}
