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
  }
}
