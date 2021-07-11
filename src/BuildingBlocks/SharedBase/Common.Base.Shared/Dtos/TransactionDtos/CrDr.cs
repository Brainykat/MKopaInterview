using System;

namespace Common.Base.Shared.Dtos.TransactionDtos
{
  public class CrDr
  {
    public static CrDr Create(Guid userId, WalletDto credit, WalletDto debit) =>
      new CrDr(userId, credit, debit);
    //Public cause of Rabbit MQ Deserialize
    public CrDr(Guid userId, WalletDto credit, WalletDto debit)
    {
      if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
      if (credit.Credit < 0) throw new ArgumentOutOfRangeException(nameof(credit));
      if (debit.Debit < 0) throw new ArgumentOutOfRangeException(nameof(debit));
      if (debit.Debit != credit.Credit) throw new ArgumentOutOfRangeException("Transaction not Balanced");
      UserId = userId;
      Credit = credit ?? throw new ArgumentNullException(nameof(credit));
      Debit = debit ?? throw new ArgumentNullException(nameof(debit));
    }
    public CrDr() { }
    public Guid UserId { get; set; }
    public WalletDto Credit { get; set; }
    public WalletDto Debit { get; set; }
  }
}
