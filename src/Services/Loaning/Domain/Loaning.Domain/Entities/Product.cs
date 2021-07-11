using Common.Base.Shared;
using Common.Base.Shared.Enums;
using Common.Base.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loaning.Domain.Entities
{
  public class Product : EntityBase
  {
    public static Product Create(string name, string description, ProductType type) =>
      new Product(name, description, type);
    public void AddLoan(Guid customerId, decimal maximumRepaymentPeriod, decimal defaultRate, Money maximumValue)
      => Loans.Add(Loan.Create(customerId, Id, maximumRepaymentPeriod, defaultRate,maximumValue));
    private Product(string name, string description, ProductType type)
    {
      Name = name ?? throw new ArgumentNullException(nameof(name));
      Description = description ?? throw new ArgumentNullException(nameof(description));
      Type = type;
    }
    private Product() { }
    public string Name { get; set; }
    public string Description { get; set; }
    public ProductType   Type { get; set; }
    public ICollection<Loan> Loans { get; set; }
  }
}
