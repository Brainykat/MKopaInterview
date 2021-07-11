namespace EventBus.Common.Shared.Configurations
{
  public class RabbitMQConfiguration
  {
    public string HostName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public int EventBusRetryCount { get; set; }
    public string SubscriptionClientName { get; set; }
    public string QueueName { get; set; }
    public string RouteKey { get; set; }
    public string Exchange { get; set; }
    //Optional
    public string LedgerExchangeName { get; set; }
    public string LedgerCrDrQue { get; set; }
    public string LedgerListCrDrQue { get; set; }
    public string LedgerCrListDrQue { get; set; }
    public string LedgerListCrListDrQue { get; set; }

    public string CrDrRoutingKey { get; set; }
    public string ListCrDrRoutingKey { get; set; }
    public string CrListDrRoutingKey { get; set; }
    public string ListCrListDrRoutingKey { get; set; }
  }
}
