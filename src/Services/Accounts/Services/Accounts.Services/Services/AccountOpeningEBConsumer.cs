using Accounts.Domain.Entities;
using EventBus.Common.Shared.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accounts.Services.Services
{
  public class AccountOpeningEBConsumer : IHostedService, IDisposable
  {
    public IServiceProvider Services { get; }
    public object JsonConvert { get; private set; }

    private readonly ILogger<AccountOpeningEBConsumer> logger;
    private readonly RabbitMQConfiguration configs;
    private IModel _channel;
    private IConnection _connection;
    public AccountOpeningEBConsumer(IServiceProvider services, ILogger<AccountOpeningEBConsumer> logger,
      IOptions<RabbitMQConfiguration> optionsMonitor)
    {
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.configs = optionsMonitor.Value;
      Services = services;
      InitializeRabbitMqListener();
    }
    private void InitializeRabbitMqListener()
    {
      try
      {
        //var con = configuration.GetSection(nameof(RabbitMQConfiguration)).<RabbitMQConfiguration>();
        var factory = new ConnectionFactory
        {
          HostName = configs.HostName,
          UserName = configs.UserName,
          Password = configs.Password,
          Port = configs.Port
        };

        _connection = factory.CreateConnection();
        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        _channel = _connection.CreateModel();
        //_channel.QueueDeclare(queue: rabbitMQConfiguration.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
      }

    }
    public async Task Register()
    {
      try
      {
        _channel.ExchangeDeclare(exchange: "message", type: "topic");
        _channel.QueueDeclare(queue: configs.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        _channel.QueueBind(queue: configs.QueueName,
                          exchange: "message",
                          routingKey: configs.RouteKey);
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (ch, ea) =>
        {
          var content = Encoding.UTF8.GetString(ea.Body.ToArray());
          logger.LogInformation("New Event on ClientPaymentQue Queued message {j}", content);
          //var queDto = JsonConvert.DeserializeObject<ClientPaymentRabbitDto>(content);

          var consume = await HandleMessage(queDto);
          if (consume)
          {
            _channel.BasicAck(ea.DeliveryTag, false);
          }
        };
        consumer.Shutdown += OnConsumerShutdown;
        consumer.Registered += OnConsumerRegistered;
        consumer.Unregistered += OnConsumerUnregistered;
        consumer.ConsumerCancelled += OnConsumerCancelled;
        //_channel.BasicConsume(configs.QueueName, false, consumer);
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
      }
      return;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
      await Register();
      return;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _connection.Close();
      return Task.CompletedTask;
    }
    private async Task<bool> HandleMessage(object dto)
    {
      try
      {
        //UNDONE: Open Accounts
        return true;
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return false;
      }
    }
    
    private void OnConsumerCancelled(object sender, ConsumerEventArgs e)
    {
    }

    private void OnConsumerUnregistered(object sender, ConsumerEventArgs e)
    {
    }

    private void OnConsumerRegistered(object sender, ConsumerEventArgs e)
    {
    }

    private void OnConsumerShutdown(object sender, ShutdownEventArgs e)
    {
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
    }

    public void Dispose()
    {
      _channel.Close();
      _connection.Close();

    }
  }
}
