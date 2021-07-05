using System.ComponentModel;

namespace Common.Base.Shared.Enums
{
  public enum AccountBearerType
  {
    Office,
    Customer,
    [Description("Mobile Network Operator")]
    MNO,
    [Description("Bank")]
    Bank
  }
}
