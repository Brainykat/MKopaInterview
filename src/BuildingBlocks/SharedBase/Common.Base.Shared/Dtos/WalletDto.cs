using Common.Base.Shared.AnnotationValidators;
using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Base.Shared.Dtos
{
  public class WalletDto
  {
    public static WalletDto CreateCreditDto(string currency, decimal credit, Guid txnRefrence, string narration, Guid accountId)
      => new WalletDto(currency, credit, 0M, txnRefrence, narration, accountId);
    public static WalletDto CreateDebitDto(string currency, decimal debit, Guid txnRefrence, string narration, Guid accountId)
      => new WalletDto(currency, 0M, debit, txnRefrence, narration, accountId);
    private WalletDto(string currency, decimal credit, decimal debit, Guid txnRefrence, string narration, Guid accountId)
    {
      Currency = currency ?? throw new ArgumentNullException(nameof(currency));
      Credit = credit;
      Debit = debit;
      TxnRefrence = txnRefrence;
      Narration = narration ?? throw new ArgumentNullException(nameof(narration));
      AccountId = accountId;
    }
    private WalletDto() { }
    [Required]
    [MaxLength(5)]
    public string Currency { get; set; }
    [Range(0, 99999999)]
    public decimal Credit { get; set; }
    [Range(0, 99999999)]
    public decimal Debit { get; set; }
    public Guid TxnRefrence { get; set; }
    [Required]
    [MaxLength(100)]
    public string Narration { get; set; }
    [NotEmpty]
    public Guid AccountId { get; set; }
  }
}
