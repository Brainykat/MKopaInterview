namespace Customers.Services.Interfaces
{
  public interface ICustomerEventProducer
  {
    bool RaisePaymentEvent(object dto);
  }
}
