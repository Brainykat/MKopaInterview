using Common.Shared.Services;
using Customers.Services.Interfaces;
using EventBus.Common.Shared.Configurations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Reflection;
using System.Text;

namespace Customers.Services.EBServices
{


  public class CustomerEventProducer : ICustomerEventProducer
  {
    private readonly ILogger<CustomerEventProducer> logger;
    private readonly RabbitMQConfiguration rabbitMQConfiguration;

    public CustomerEventProducer(ILogger<CustomerEventProducer> logger, RabbitMQConfiguration rabbitMQConfiguration)
    {
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.rabbitMQConfiguration = rabbitMQConfiguration ?? throw new ArgumentNullException(nameof(rabbitMQConfiguration));
    }
    public bool RaisePaymentEvent(object dto)
    {
      try
      {
        var factory = new ConnectionFactory()
        {
          HostName = rabbitMQConfiguration.HostName,
          Password = rabbitMQConfiguration.Password,
          UserName = rabbitMQConfiguration.UserName,
          Port = rabbitMQConfiguration.Port
        };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
          channel.QueueDeclare(queue: rabbitMQConfiguration.QueueName, // "ClientPaymentQue",
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

          var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto));
          channel.BasicPublish(exchange: "",
                               routingKey: rabbitMQConfiguration.QueueName,
                               basicProperties: null,
                               body: body);
          logger.LogInformation("in {i} Queued message {j}", rabbitMQConfiguration.QueueName, JsonConvert.SerializeObject(dto));
          return true;
        }
      }
      catch (System.Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return false;
      }
    }
  }
}
