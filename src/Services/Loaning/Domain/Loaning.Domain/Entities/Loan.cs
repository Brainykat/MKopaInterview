using Common.Base.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loaning.Domain.Entities
{
  public class Loan : EntityBase
  {
    public Guid CustomerId { get; set; }
    public Guid ProductId { get; set; }
    public decimal MaximumRepaymentPeriod { get; set; }
    public decimal DefaultRate { get; set; }

    public Product Product { get; set; }
  }
}
