using System;
using System.Collections.Generic;

namespace Common.Base.Shared.Dtos.TransactionDtos
{
  public class ListCrListDr
  {
    public Guid UserId { get; set; }
    public ICollection<WalletDto> Credits { get; set; }
    public ICollection<WalletDto> Debits { get; set; }
  }
}
