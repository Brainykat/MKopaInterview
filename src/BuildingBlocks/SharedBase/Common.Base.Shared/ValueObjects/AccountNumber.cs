using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Base.Shared.ValueObjects
{
  public class AccountNumber
  {
    private AccountNumber(int cOACODE, long number, string bearerCode)
    {
      if (string.IsNullOrWhiteSpace(bearerCode)) throw new ArgumentNullException(nameof(bearerCode));
      if (cOACODE <= 0) throw new ArgumentOutOfRangeException(nameof(cOACODE));
      if (number <= 0) throw new ArgumentOutOfRangeException(nameof(number));
      COACODE = cOACODE;
      Number = number;
      BearerCode = bearerCode;
    }
    public static AccountNumber Create(int cOACODE, long number, string bearerCode) =>
        new AccountNumber(cOACODE, number, bearerCode);
    private AccountNumber() { }
    /// <summary>
    /// 1001 - 1999 => Office Accounts
    /// 2000 - 2999 => Customer
    /// 3000 - 3999 => MNO
    /// 4000 - 4999 => Bank
    /// 5000 - 5999
    /// </summary>
    public int COACODE { get; private set; }
    public long Number { get; private set; }
    public string BearerCode { get; private set; }
    public override string ToString() => $"{COACODE}{Number.ToString().PadLeft(6, '0')}{BearerCode}";

    public override bool Equals(object obj)
    {
      return obj is AccountNumber number &&
             COACODE == number.COACODE &&
             Number == number.Number &&
             BearerCode == number.BearerCode;
    }

    public override int GetHashCode()
    {
      var hashCode = 1251919988;
      hashCode = hashCode * -1521134295 + COACODE.GetHashCode();
      hashCode = hashCode * -1521134295 + Number.GetHashCode();
      hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(BearerCode);
      return hashCode;
    }

    public static bool operator ==(AccountNumber left, AccountNumber right)
    {
      return EqualityComparer<AccountNumber>.Default.Equals(left, right);
    }

    public static bool operator !=(AccountNumber left, AccountNumber right)
    {
      return !(left == right);
    }
  }
}
