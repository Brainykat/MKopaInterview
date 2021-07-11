using Ledgers.Domain.Entities;
using Ledgers.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ledgers.Data.Repositories
{
  public class LedgerRepository : ILedgerRepository
  {
    private readonly LedgerContext context;
    public LedgerRepository(LedgerContext context)
    {
      this.context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<List<Wallet>> GetLastLedgerEntries(Guid accountId, int numberOfRows) =>
      await context.Wallets.AsNoTracking().Where(l => l.AccountId == accountId).OrderByDescending(i => i.TxnTime).Take(numberOfRows).ToListAsync();
    //public async Task<List<Wallet>> GetLastLedgerEntriesUsingAccountId(Guid accountId, int numberOfRows) =>
    //  await context.Wallets.AsNoTracking().Where(l => l.AccountId == accountId).OrderByDescending(i => i.TxnTime).Take(numberOfRows).ToListAsync();
    public async Task<List<Wallet>> GetLastLedgertEntriesForASpecificRangeUsingAccountId(Guid accountId, DateTime from, DateTime to) =>
      await context.Wallets.AsNoTracking().Where(l => l.AccountId == accountId && l.TxnTime >= from && l.TxnTime <= to).ToListAsync();
    public async Task<List<Wallet>> GetSpecialLedgerEntriesNoTracking(Expression<Func<Wallet, bool>> predicate) =>
      await context.Wallets.AsNoTracking().Where(predicate).ToListAsync();

    public async Task Add(Wallet debit, Wallet credit)
    {
      if (ValidateTxn(debit.Debit.Amount, credit.Credit.Amount))
      {
        context.Wallets.Add(debit);
        context.Wallets.Add(credit);
        await context.SaveChangesAsync();
      }
    }
    public async Task Add(List<Wallet> debits, Wallet credit)
    {
      if (ValidateTxn(debits.Sum(a => a.Debit.Amount), credit.Credit.Amount))
      {
        context.Wallets.AddRange(debits);
        context.Wallets.Add(credit);
        await context.SaveChangesAsync();
      }
    }
    public async Task Add(Wallet debit, List<Wallet> credits)
    {
      if (ValidateTxn(debit.Debit.Amount, credits.Sum(c => c.Credit.Amount)))
      {
        context.Wallets.Add(debit);
        context.Wallets.AddRange(credits);
        await context.SaveChangesAsync();
      }
    }
    public async Task Add(List<Wallet> debits, List<Wallet> credits)
    {
      if (ValidateTxn(debits.Sum(d => d.Debit.Amount), credits.Sum(c => c.Credit.Amount)))
      {
        context.Wallets.AddRange(debits);
        context.Wallets.AddRange(credits);
        await context.SaveChangesAsync();
      }
    }
    private bool ValidateTxn(decimal debit, decimal credit) => debit == credit;
  }
}
