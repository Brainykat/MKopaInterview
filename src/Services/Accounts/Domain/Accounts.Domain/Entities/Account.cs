using Common.Base.Shared;
using Common.Base.Shared.Enums;
using Common.Base.Shared.ValueObjects;
using System;

namespace Accounts.Domain.Entities
{
  public class Account : EntityBase
  {
    internal static Account Create(string name, AccountNumber accountNumber, AccountBearerType accountBearerType,
        Guid chartOfAccountId, Money accountTransactionLimit, int signatories, Guid? bearerId = default) =>
        new Account(name, accountNumber, accountBearerType,
            chartOfAccountId, accountTransactionLimit, signatories, bearerId);
    public void UpdateBalance(Money balance) => Balance = balance;
    private Account() { }
    private Account(string name, AccountNumber accountNumber, AccountBearerType accountBearerType,
        Guid chartOfAccountId, Money accountTransactionLimit, int signatories, Guid? bearerId = default)
    {
      if (accountBearerType != AccountBearerType.Office && bearerId == Guid.Empty)
        throw new Exception("An office account must have a bearer");
      if ((accountBearerType == AccountBearerType.Customer || accountBearerType == AccountBearerType.Bank||
        accountBearerType == AccountBearerType.MNO) && bearerId == Guid.Empty)
        throw new Exception("Client none office account must have an affiliation Id");
      if (signatories <= 0) throw new ArgumentOutOfRangeException(nameof(signatories));
      if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
      Name = name;
      AccountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber));
      AccountBearerType = accountBearerType;
      CreatedOn = DateTime.UtcNow;
      ChartOfAccountId = chartOfAccountId;
      AccountTransactionLimit = accountTransactionLimit ?? throw new ArgumentNullException(nameof(accountTransactionLimit));
      GenerateNewIdentity();
      Signatories = signatories;
      BearerId = bearerId;
      Balance = Money.Create(accountTransactionLimit.Currency, 0M);
    }
    public Guid? BearerId { get; set; }
    public string Name { get; set; }
    public AccountNumber AccountNumber { get; set; }
    public AccountBearerType AccountBearerType { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid ChartOfAccountId { get; set; }
    public Money AccountTransactionLimit { get; set; }
    public int Signatories { get; set; }
    public ChartOfAccount ChartOfAccount { get; set; }
    public Money Balance { get; set; }
  }
}
