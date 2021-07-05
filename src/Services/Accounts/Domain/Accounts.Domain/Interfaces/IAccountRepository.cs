using Accounts.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Accounts.Domain.Interfaces
{
  public interface IAccountRepository
  {
    Task Add(Account account);
    Task Add(List<Account> accounts);
    Task<long> CurrentCount(int chartCode, string bearer);
    Task<Account> Find(string name);
    Task<Account> GetAccountNoTracking(Guid id);
    Task<List<Account>> GetAccounts();
    Task<Account> GetAccountTracked(Guid id);
    Task<Account> GetCUstomerAccountReceivable(Guid id);
    Task<List<Account>> GetCustomerAccounts(Guid id);
    Task<List<Account>> OfficeAccounts();
    Task Update(Account account);
  }
}
