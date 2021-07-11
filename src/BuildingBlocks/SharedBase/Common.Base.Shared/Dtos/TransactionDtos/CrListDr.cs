using System;
using System.Collections.Generic;

namespace Common.Base.Shared.Dtos.TransactionDtos
{
  public class CrListDr
  {
    public Guid UserId { get; set; }
    public WalletDto Credit { get; set; }
    public ICollection<WalletDto> Debits { get; set; }
  }
}
