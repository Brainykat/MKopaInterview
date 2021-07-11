using System;
using System.Collections.Generic;

namespace Common.Base.Shared.Dtos.TransactionDtos
{
  public class ListCrDr
  {
    public Guid UserId { get; set; }
    public ICollection<WalletDto> Credits { get; set; }
    public WalletDto Debit { get; set; }
  }
}
