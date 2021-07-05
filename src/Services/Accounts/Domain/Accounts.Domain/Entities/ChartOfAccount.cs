using Common.Base.Shared;
using Common.Base.Shared.Enums;
using Common.Base.Shared.ValueObjects;
using System;
using System.Collections.Generic;

namespace Accounts.Domain.Entities
{
  public class ChartOfAccount : EntityBase
  {
    private ChartOfAccount() { }
    private ChartOfAccount(int number, string description, AccountType accountType, Statement statement) : base()
    {
      if (string.IsNullOrWhiteSpace(description)) throw new ArgumentNullException(nameof(description));
      if (number <= 0) throw new ArgumentOutOfRangeException(nameof(number));
      Number = number;
      Description = description;
      AccountType = accountType;
      Statement = statement;
    }
    public static ChartOfAccount Create(int number, string description, AccountType accountType, Statement statement) =>
        new ChartOfAccount(number, description, accountType, statement);
    public void OpenAccount(string name, AccountNumber accountNumber, AccountBearerType accountBearerType,
         Money accountTransactionLimit, int signatories, Guid? bearerId = default) =>
      Accounts.Add(Account.Create(name, accountNumber, accountBearerType, Id, accountTransactionLimit, signatories, bearerId));
    public int Number { get; set; }
    public string Description { get; set; }
    public AccountType AccountType { get; set; }
    public Statement Statement { get; set; }
    public ICollection<Account> Accounts { get; set; }
  }
}
