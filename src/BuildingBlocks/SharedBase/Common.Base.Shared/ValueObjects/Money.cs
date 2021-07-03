using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Base.Shared.ValueObjects
{
  public class Money
  {
    private Money() { }
    private Money(string currency, decimal amount, DateTime time)
    {
      if (string.IsNullOrWhiteSpace(currency)) throw new ArgumentNullException(nameof(currency));
      if (amount < 0M) throw new ArgumentOutOfRangeException(nameof(amount));
      Currency = currency.Trim().ToUpper(); Amount = amount; Time = time;
    }
    public static Money Create(string currency, decimal amount, DateTime? time = null) =>
        new Money(currency, amount, time != null ? time.Value : DateTime.UtcNow);
    
    public string Currency { get;  } // The setters are for elastic De-serialize only
    public decimal Amount { get;  }
    public DateTime Time { get;  }
    public override string ToString() => $"{Currency} {Amount:N2}";
    public override bool Equals(object obj)
    {
      return obj is Money money &&
             Currency == money.Currency &&
             Amount == money.Amount &&
             Time == money.Time;
    }
    public override int GetHashCode()
    {
      var hashCode = 624119691;
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Currency);
      hashCode = hashCode * -1521134295 + Amount.GetHashCode();
      hashCode = hashCode * -1521134295 + Time.GetHashCode();
      return hashCode;
    }
    public static bool operator ==(Money left, Money right)
    {
      return EqualityComparer<Money>.Default.Equals(left, right);
    }
    public static bool operator !=(Money left, Money right)
    {
      return !(left == right);
    }
  }
}
