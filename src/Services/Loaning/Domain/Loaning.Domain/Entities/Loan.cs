using Common.Base.Shared;
using Common.Base.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loaning.Domain.Entities
{
  public class Loan : EntityBase
  {
    internal static Loan Create(Guid customerId, Guid productId, decimal maximumRepaymentPeriod, decimal defaultRate, Money maximumValue)
      => new Loan(customerId, productId, maximumRepaymentPeriod, defaultRate,maximumValue);
    private Loan(Guid customerId, Guid productId, decimal maximumRepaymentPeriod, decimal defaultRate, Money maximumValue)
    {
      //TODO on service layer validate Customer Exists Via event bus
      if (customerId == Guid.Empty) throw new ArgumentNullException("Invalid customer Id");
      CustomerId = customerId;
      if (productId == Guid.Empty) throw new ArgumentNullException("Invalid product Id");
      ProductId = productId;
      MaximumRepaymentPeriod = maximumRepaymentPeriod;
      if (DefaultRate <= 0) throw new ArgumentException("Invalid Loan Rate");
      DefaultRate = defaultRate;
      MaximumValue = maximumValue;
    }
    private Loan() { }
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public decimal MaximumRepaymentPeriod { get; set; }
    public decimal DefaultRate { get; set; }
    //....
    public Money MaximumValue { get; set; }
    public Product Product { get; set; }
  }
}
