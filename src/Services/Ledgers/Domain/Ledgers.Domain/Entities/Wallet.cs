using Common.Base.Shared;
using Common.Base.Shared.ValueObjects;
using System;

namespace Ledgers.Domain.Entities
{
  public class Wallet : EntityBase
  {
    public static Wallet Create(Guid accountId, Money credit, Money debit,
            Guid txnRefrence, string narration, Guid transactingUser) =>
            new Wallet(accountId, credit, debit, txnRefrence, narration, transactingUser);
    public static Wallet Create(Guid accountId, Money credit, Money debit,
            Guid txnRefrence, string narration, Guid transactingUser, DateTime txnTime) =>
            new Wallet(accountId, credit, debit, txnRefrence, narration, transactingUser, txnTime);
    private Wallet() { }
    private Wallet(Guid accountId, Money credit, Money debit,
        Guid txnRefrence, string narration, Guid transactingUser)
    {
      Credit = credit ?? throw new ArgumentNullException(nameof(credit));
      Debit = debit ?? throw new ArgumentNullException(nameof(debit));
      if (credit.Amount < 0) throw new Exception("Invalid Entry Credit is less than zero");
      if (debit.Amount < 0) throw new Exception("Invalid Entry Debit is less than zero");
      if (credit.Amount == 0 && debit.Amount == 0) if (credit.Amount < 0) throw new Exception("Invalid Entry both Credit and Debit cannot be zero");
      if (credit.Amount == debit.Amount) throw new Exception("Invalid Entry Debit Equal Credit");
      if (credit.Amount < 0 && debit.Amount < 0) if (credit.Amount < 0) throw new Exception("Invalid Entry both Credit and Debit cannot be greater than zero");
      if (string.IsNullOrWhiteSpace(narration)) throw new ArgumentNullException(nameof(narration));
      if (txnRefrence == Guid.Empty) throw new ArgumentNullException(nameof(txnRefrence));
      if (transactingUser == Guid.Empty) throw new ArgumentNullException(nameof(transactingUser));
      AccountId = accountId;
      TxnTime = DateTime.UtcNow;
      TxnRefrence = txnRefrence;
      Narration = narration;
      TransactingUser = transactingUser;
    }
    private Wallet(Guid accountId, Money credit, Money debit,
        Guid txnRefrence, string narration, Guid transactingUser, DateTime txnTime)
    {
      Credit = credit ?? throw new ArgumentNullException(nameof(credit));
      Debit = debit ?? throw new ArgumentNullException(nameof(debit));
      if (credit.Amount < 0) throw new Exception("Invalid Entry Credit is less than zero");
      if (debit.Amount < 0) throw new Exception("Invalid Entry Debit is less than zero");
      if (credit.Amount == 0 && debit.Amount == 0) if (credit.Amount < 0) throw new Exception("Invalid Entry both Credit and Debit cannot be zero");
      if (credit.Amount == debit.Amount) throw new Exception("Invalid Entry Debit Equal Credit");
      if (string.IsNullOrWhiteSpace(narration)) throw new ArgumentNullException(nameof(narration));
      if (txnRefrence == Guid.Empty) throw new ArgumentNullException(nameof(txnRefrence));
      if (transactingUser == Guid.Empty) throw new ArgumentNullException(nameof(transactingUser));
      if (txnTime > DateTime.UtcNow) throw new Exception("Front dated transaction not allowed");
      AccountId = accountId;
      TxnTime = txnTime;
      TxnRefrence = txnRefrence;
      Narration = narration;
      TransactingUser = transactingUser;
    }
    public DateTime TxnTime { get; }
    public Money Credit { get; }
    public Money Debit { get; set; }
    public Guid TxnRefrence { get; set; }
    public string Narration { get; set; }
    public Guid TransactingUser { get; set; }
    public Guid AccountId { get; set; }
  }
}
