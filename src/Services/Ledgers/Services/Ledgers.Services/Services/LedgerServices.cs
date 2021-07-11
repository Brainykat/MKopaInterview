using Common.Base.Shared.Dtos;
using Common.Base.Shared.ValueObjects;
using Common.Base.Shared.ViewModels;
using Common.Shared.Services;
using Ledgers.Domain.Entities;
using Ledgers.Domain.Interfaces;
using Ledgers.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Ledgers.Services.Services
{
  public class LedgerServices : ILedgerServices
  {
    private readonly ILedgerRepository repository;
    private readonly ILogger<LedgerServices> logger;
    public LedgerServices(ILedgerRepository repository, ILogger<LedgerServices> logger)
    {
      this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public async Task<ICollection<Wallet>> GetLastLedgerEntries(Guid ledgerAccountId, int numberOfRows) => await repository.GetLastLedgerEntries(ledgerAccountId, numberOfRows);
    public async Task<ICollection<Wallet>> GetLastLedgertEntriesForASpecificRangeUsingAccountId(Guid accountId, DateTime from, DateTime to) =>
      await repository.GetLastLedgertEntriesForASpecificRangeUsingAccountId(accountId, from, to);
    public async Task<APIResponse> Transact(WalletDto debitDto, WalletDto creditDto, Guid user)
    {
      try
      {
        var debit = MakeDebitEntry(debitDto, user);
        var credit = MakeCreditEntry(creditDto, user);
        if (ValidateTxn(debit.Debit.Amount, credit.Credit.Amount))
        {
          await repository.Add(debit, credit);
          //UNDONE: Raise Balance Update Event
          return new APIResponse();
        }
        return new APIResponse(false, new List<string> { "Transaction doesn't balance" });
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return new APIResponse(ex.GetRealException());
      }
    }
    public async Task<APIResponse> Transact(ICollection<WalletDto> debitDtos, WalletDto creditDto, Guid user)
    {
      try
      {
        List<Wallet> debits = (from debitDto in debitDtos
                               select MakeCreditEntry(debitDto, user)).ToList();
        var credit = MakeDebitEntry(creditDto, user);
        if (ValidateTxn(debits.Sum(a => a.Debit.Amount), credit.Credit.Amount))
        {
          await repository.Add(debits, credit);//UNDONE: Raise Balance Update Event
          return new APIResponse();
        }
        return new APIResponse(false, new List<string> { "Transaction doesn't balance" });
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return new APIResponse(ex.GetRealException());
      }
    }
    public async Task<APIResponse> Transact(ICollection<WalletDto> debitDtos, ICollection<WalletDto> creditDtos, Guid user)
    {
      try
      {
        List<Wallet> debits = (from debitDto in debitDtos
                               select MakeCreditEntry(debitDto, user)).ToList();
        List<Wallet> credits = (from creditDto in creditDtos
                                select MakeCreditEntry(creditDto, user)).ToList();
        if (ValidateTxn(debits.Sum(d => d.Debit.Amount), credits.Sum(c => c.Credit.Amount)))
        {
          await repository.Add(debits, credits);//UNDONE: Raise Balance Update Event
          return new APIResponse();
        }
        return new APIResponse(false, new List<string> { "Transaction doesn't balance" });
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return new APIResponse(ex.GetRealException());
      }
    }
    public async Task<APIResponse> Transact(WalletDto debitDto, ICollection<WalletDto> creditDtos, Guid user)
    {
      try
      {
        var debit = MakeDebitEntry(debitDto, user);
        List<Wallet> credits = (from creditDto in creditDtos
                                select MakeCreditEntry(creditDto, user)).ToList();
        if (ValidateTxn(debit.Debit.Amount, credits.Sum(c => c.Credit.Amount)))
        {
          await repository.Add(debit, credits);//UNDONE: Raise Balance Update Event
          return new APIResponse();
        }
        return new APIResponse(false, new List<string> { "Transaction doesn't balance" });
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return new APIResponse(ex.GetRealException());
      }
    }
    private bool ValidateTxn(decimal debit, decimal credit) => debit == credit;
    private Wallet MakeCreditEntry(WalletDto dto, Guid user)
    {
      try
      {
        if (dto.Credit <= 0) throw new Exception("Invalid Wallet Credit Entry");
        var credit = Money.Create(dto.Currency, dto.Credit);
        var debit = Money.Create(dto.Currency, 0M);
        return Wallet.Create(dto.AccountId, credit, debit, dto.TxnRefrence, dto.Narration, user);
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return null;
      }

    }
    private Wallet MakeDebitEntry(WalletDto dto, Guid user)
    {
      try
      {
        if (dto.Debit <= 0) throw new Exception("Invalid Wallet Debit Entry");
        var debit = Money.Create(dto.Currency, dto.Debit);
        var credit = Money.Create(dto.Currency, 0M);
        return Wallet.Create(dto.AccountId, credit, debit, dto.TxnRefrence, dto.Narration, user);
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return null;
      }

    }
    #region Backdated Transaction
    public async Task<APIResponse> Transact(WalletDto debitDto, WalletDto creditDto, Guid user, DateTime txnTime)
    {
      try
      {
        var debit = MakeDebitEntry(debitDto, user, txnTime);
        var credit = MakeDebitEntry(creditDto, user, txnTime);
        if (ValidateTxn(debit.Debit.Amount, credit.Credit.Amount))
        {
          await repository.Add(debit, credit);//UNDONE: Raise Balance Update Event
          return new APIResponse();
        }
        return new APIResponse(false, new List<string> { "Transaction doesn't balance" });
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return new APIResponse(ex.GetRealException());
      }
    }
    public async Task<APIResponse> Transact(ICollection<WalletDto> debitDtos, WalletDto creditDto, Guid user, DateTime txnTime)
    {
      try
      {
        List<Wallet> debits = (from debitDto in debitDtos
                               select MakeCreditEntry(debitDto, user, txnTime)).ToList();
        var credit = MakeDebitEntry(creditDto, user, txnTime);
        if (ValidateTxn(debits.Sum(a => a.Debit.Amount), credit.Credit.Amount))
        {
          await repository.Add(debits, credit);//UNDONE: Raise Balance Update Event
          return new APIResponse();
        }
        return new APIResponse(false, new List<string> { "Transaction doesn't balance" });
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return new APIResponse(ex.GetRealException());
      }
    }
    public async Task<APIResponse> Transact(ICollection<WalletDto> debitDtos, ICollection<WalletDto> creditDtos, Guid user, DateTime txnTime)
    {
      try
      {
        List<Wallet> debits = (from debitDto in debitDtos
                               select MakeCreditEntry(debitDto, user, txnTime)).ToList();
        List<Wallet> credits = (from creditDto in creditDtos
                                select MakeCreditEntry(creditDto, user, txnTime)).ToList();
        if (ValidateTxn(debits.Sum(d => d.Debit.Amount), credits.Sum(c => c.Credit.Amount)))
        {
          await repository.Add(debits, credits);//UNDONE: Raise Balance Update Event
          return new APIResponse();
        }
        return new APIResponse(false, new List<string> { "Transaction doesn't balance" });
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return new APIResponse(ex.GetRealException());
      }
    }
    public async Task<APIResponse> Transact(WalletDto debitDto, ICollection<WalletDto> creditDtos, Guid user, DateTime txnTime)
    {
      try
      {
        var debit = MakeDebitEntry(debitDto, user, txnTime);
        List<Wallet> credits = (from creditDto in creditDtos
                                select MakeCreditEntry(creditDto, user, txnTime)).ToList();
        if (ValidateTxn(debit.Debit.Amount, credits.Sum(c => c.Credit.Amount)))
        {
          await repository.Add(debit, credits);//UNDONE: Raise Balance Update Event
          return new APIResponse();
        }
        return new APIResponse(false, new List<string> { "Transaction doesn't balance" });
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return new APIResponse(ex.GetRealException());
      }
    }
    private Wallet MakeCreditEntry(WalletDto dto, Guid user, DateTime txnTime)
    {
      try
      {
        if (dto.Credit < 0) throw new Exception("Invalid Wallet Credit Entry");
        var credit = Money.Create(dto.Currency, dto.Credit);
        var debit = Money.Create(dto.Currency, 0M);
        return Wallet.Create(dto.AccountId, credit, debit, dto.TxnRefrence, dto.Narration, user, txnTime);
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return null;
      }

    }
    private Wallet MakeDebitEntry(WalletDto dto, Guid user, DateTime txnTime)
    {
      try
      {
        if (dto.Debit <= 0) throw new Exception("Invalid Wallet Debit Entry");
        var debit = Money.Create(dto.Currency, dto.Debit);
        var credit = Money.Create(dto.Currency, 0M);
        return Wallet.Create(dto.AccountId, credit, debit, dto.TxnRefrence, dto.Narration, user, txnTime);
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return null;
      }

    }
    #endregion
  }
}
