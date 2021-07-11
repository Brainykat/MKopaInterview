using Common.Base.Shared.Dtos;
using Common.Base.Shared.ViewModels;
using Ledgers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ledgers.Services.Interfaces
{
  public interface ILedgerServices
  {
    Task<ICollection<Wallet>> GetLastLedgerEntries(Guid ledgerAccountId, int numberOfRows);
    Task<ICollection<Wallet>> GetLastLedgertEntriesForASpecificRangeUsingAccountId(Guid accountId, DateTime from, DateTime to);
    Task<APIResponse> Transact(ICollection<WalletDto> debitDtos, ICollection<WalletDto> creditDtos, Guid user);
    Task<APIResponse> Transact(ICollection<WalletDto> debitDtos, ICollection<WalletDto> creditDtos, Guid user, DateTime txnTime);
    Task<APIResponse> Transact(ICollection<WalletDto> debitDtos, WalletDto creditDto, Guid user);
    Task<APIResponse> Transact(ICollection<WalletDto> debitDtos, WalletDto creditDto, Guid user, DateTime txnTime);
    Task<APIResponse> Transact(WalletDto debitDto, ICollection<WalletDto> creditDtos, Guid user);
    Task<APIResponse> Transact(WalletDto debitDto, ICollection<WalletDto> creditDtos, Guid user, DateTime txnTime);
    Task<APIResponse> Transact(WalletDto debitDto, WalletDto creditDto, Guid user);
    Task<APIResponse> Transact(WalletDto debitDto, WalletDto creditDto, Guid user, DateTime txnTime);
  }
}
