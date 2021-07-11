using Ledgers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ledgers.Domain.Interfaces
{
  public interface ILedgerRepository
  {
    Task Add(List<Wallet> debits, List<Wallet> credits);
    Task Add(List<Wallet> debits, Wallet credit);
    Task Add(Wallet debit, List<Wallet> credits);
    Task Add(Wallet debit, Wallet credit);
    Task<List<Wallet>> GetLastLedgerEntries(Guid accountId, int numberOfRows);
    Task<List<Wallet>> GetLastLedgertEntriesForASpecificRangeUsingAccountId(Guid accountId, DateTime from, DateTime to);
    Task<List<Wallet>> GetSpecialLedgerEntriesNoTracking(Expression<Func<Wallet, bool>> predicate);
  }
}
