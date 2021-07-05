using Accounts.Domain.Entities;
using Accounts.Domain.Interfaces;
using Common.Base.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Data.Repositories
{
  public class AccountRepository : IAccountRepository
  {
    private readonly AccountsContext context;
    public AccountRepository(AccountsContext context)
    {
      this.context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<List<Account>> GetAccounts() => await context.Accounts.AsNoTracking().ToListAsync();

    public async Task<Account> GetAccountTracked(Guid id) =>
        await context.Accounts.FirstOrDefaultAsync(c => c.Id == id);
    public async Task<Account> GetAccountNoTracking(Guid id) =>
        await context.Accounts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    public async Task<List<Account>> OfficeAccounts() => await context.Accounts.AsNoTracking().
        Where(a => a.AccountBearerType == AccountBearerType.Office).ToListAsync();

    public async Task<Account> GetCUstomerAccountReceivable(Guid id) => await context.Accounts.AsNoTracking()
      .FirstOrDefaultAsync(c => c.AccountNumber.COACODE == 2010 && c.BearerId == id);

    public async Task<List<Account>> GetCustomerAccounts(Guid id) => await context.Accounts.AsNoTracking().
        Where(a => a.AccountBearerType == AccountBearerType.Customer && a.BearerId == id).ToListAsync();

    public async Task<Account> Find(string name) =>
        await context.Accounts.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name);



    public async Task Add(Account account)
    {
      context.Accounts.Add(account);
      await context.SaveChangesAsync();
    }
    public async Task Add(List<Account> accounts)
    {
      context.Accounts.AddRange(accounts);
      await context.SaveChangesAsync();
    }
    public async Task Update(Account account)
    {
      context.Accounts.Update(account);
      await context.SaveChangesAsync();
    }
    public async Task<long> CurrentCount(int chartCode, string bearer) => await context.Accounts.AsNoTracking().Where(a => a.AccountNumber.COACODE == chartCode && a.AccountNumber.BearerCode == bearer).CountAsync();
  }
}
