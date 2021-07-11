using Common.Base.Shared.Dtos.TransactionDtos;
using Common.Base.Shared.ValueObjects;
using Common.Shared.Services;
using EventBus.Common.Shared.Configurations;
using Ledgers.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ledgers.Services.EventBusServices
{
  public class LedgerEventConsumer : IHostedService
  {
    public IServiceProvider Services { get; }
    private readonly ILogger<LedgerEventConsumer> logger;
    private readonly RabbitMQConfiguration configs;
    private IModel channel;
    private IConnection connection;

    public LedgerEventConsumer(IServiceProvider services, ILogger<LedgerEventConsumer> logger, IOptionsMonitor<RabbitMQConfiguration> optionsMonitor)
    {
      Services = services ?? throw new ArgumentNullException(nameof(services));
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.configs = optionsMonitor.CurrentValue;
      InitializeRabbitMqListener();
    }
    private void InitializeRabbitMqListener()
    {
      try
      {
        var factory = new ConnectionFactory
        {
          HostName = configs.HostName,
          UserName = configs.UserName,
          Password = configs.Password,
          Port = configs.Port
        };
        connection = factory.CreateConnection();
        connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
        channel = connection.CreateModel();
      }
      catch (Exception ex)
      {
        logger.LogError($"{GetType().FullName} ==> Error {ex.Message} inner {ex.InnerException?.InnerException}");
      }
    }
    #region Register Que's
    public async Task RegisterCrDr()
    {
      try
      {
        channel.ExchangeDeclare(exchange: configs.LedgerExchangeName, type: "direct", durable: true);
        channel.QueueDeclare(queue: configs.LedgerCrDrQue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: configs.LedgerCrDrQue, exchange: configs.LedgerExchangeName, routingKey: configs.CrDrRoutingKey);
        channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(channel);
        channel.BasicConsume(configs.LedgerCrDrQue, false, consumer);
        consumer.Received += async (ch, ea) =>
        {
          var content = Encoding.UTF8.GetString(ea.Body.ToArray());
          var queDto = JsonConvert.DeserializeObject<CrDr>(content);
          var consume = await HandleMessageCrDr(queDto);
          if (consume)
          {
            channel.BasicAck(ea.DeliveryTag, false);
          }
          else
          {
            channel.BasicNack(ea.DeliveryTag, false, false);
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
      }
    }
    public async Task RegisterCrListDr()
    {
      try
      {
        channel.ExchangeDeclare(exchange: configs.LedgerExchangeName, type: "direct", durable: true);
        channel.QueueDeclare(queue: configs.LedgerCrListDrQue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: configs.LedgerCrListDrQue, exchange: configs.LedgerExchangeName, routingKey: configs.CrListDrRoutingKey);
        channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(channel);
        channel.BasicConsume(configs.LedgerCrDrQue, false, consumer);
        consumer.Received += async (ch, ea) =>
        {
          var content = Encoding.UTF8.GetString(ea.Body.ToArray());
          var queDto = JsonConvert.DeserializeObject<CrListDr>(content);
          var consume = await HandleMessageCrListDr(queDto);
          if (consume)
          {
            channel.BasicAck(ea.DeliveryTag, false);
          }
          else
          {
            channel.BasicNack(ea.DeliveryTag, false, false);
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
      }
    }
    public async Task RegisterListCrDr()
    {
      try
      {
        channel.ExchangeDeclare(exchange: configs.LedgerExchangeName, type: "direct", durable: true);
        channel.QueueDeclare(queue: configs.LedgerListCrDrQue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: configs.LedgerListCrDrQue, exchange: configs.LedgerExchangeName, routingKey: configs.ListCrDrRoutingKey);
        channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(channel);
        channel.BasicConsume(configs.LedgerCrDrQue, false, consumer);
        consumer.Received += async (ch, ea) =>
        {
          var content = Encoding.UTF8.GetString(ea.Body.ToArray());
          var queDto = JsonConvert.DeserializeObject<ListCrDr>(content);
          var consume = await HandleMessageListCrDr(queDto);
          if (consume)
          {
            channel.BasicAck(ea.DeliveryTag, false);
          }
          else
          {
            channel.BasicNack(ea.DeliveryTag, false, false);
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
      }
    }
    public async Task RegisterListCrListDr()
    {
      try
      {
        channel.ExchangeDeclare(exchange: configs.LedgerExchangeName, type: "direct", durable: true);
        channel.QueueDeclare(queue: configs.LedgerListCrListDrQue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        channel.QueueBind(queue: configs.LedgerListCrListDrQue, exchange: configs.LedgerExchangeName, routingKey: configs.ListCrListDrRoutingKey);
        channel.BasicQos(0, 1, false);

        var consumer = new EventingBasicConsumer(channel);
        channel.BasicConsume(configs.LedgerCrDrQue, false, consumer);
        consumer.Received += async (ch, ea) =>
        {
          var content = Encoding.UTF8.GetString(ea.Body.ToArray());
          var queDto = JsonConvert.DeserializeObject<ListCrListDr>(content);
          var consume = await HandleMessageListCrListDr(queDto);
          if (consume)
          {
            channel.BasicAck(ea.DeliveryTag, false);
          }
          else
          {
            channel.BasicNack(ea.DeliveryTag, false, false);
          }
        };
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
      }
    }
    #endregion
    #region Handle Messages
    private async Task<bool> HandleMessageCrDr(CrDr dto)
    {
      try
      {
        logger.LogInformation("Handling data from Ledger CrDr Que");
        logger.LogInformation("in Ledger CrDr Queued message {j}", JsonConvert.SerializeObject(dto));
        using (var scope = Services.CreateScope())
        {
          var ledgerService = scope.ServiceProvider.GetRequiredService<ILedgerServices>();
          var res = await ledgerService.Transact(dto.Debit, dto.Credit, dto.UserId);
          if (res.Status)
          {
            await UpdateElasticAccountBalance(scope, dto.Debit.AccountId, Money.Create(dto.Debit.Currency, dto.Debit.Debit), false);
            await UpdateElasticAccountBalance(scope, dto.Credit.AccountId, Money.Create(dto.Credit.Currency, dto.Credit.Credit), true);
            return true;
          }
          return false;
        }
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return false;
      }
    }
    private async Task<bool> HandleMessageCrListDr(CrListDr dto)
    {
      try
      {
        logger.LogInformation("Handling data from Ledger CrListDr Que");
        using (var scope = Services.CreateScope())
        {
          var ledgerService = scope.ServiceProvider.GetRequiredService<ILedgerServices>();
          var res = await ledgerService.Transact(dto.Debits, dto.Credit, dto.UserId);
          if (res.Status)
          {
            foreach (var debit in dto.Debits)
            {
              await UpdateElasticAccountBalance(scope, debit.AccountId, Money.Create(debit.Currency, debit.Debit), false);
            }
            await UpdateElasticAccountBalance(scope, dto.Credit.AccountId, Money.Create(dto.Credit.Currency, dto.Credit.Credit), true);
            return true;
          }
          return false;
        }
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return false;
      }
    }
    private async Task<bool> HandleMessageListCrDr(ListCrDr dto)
    {
      try
      {
        logger.LogInformation("Handling data from Ledger CrDr Que");
        using (var scope = Services.CreateScope())
        {
          var ledgerService = scope.ServiceProvider.GetRequiredService<ILedgerServices>();
          var res = await ledgerService.Transact(dto.Debit, dto.Credits, dto.UserId);
          if (res.Status)
          {
            await UpdateElasticAccountBalance(scope, dto.Debit.AccountId, Money.Create(dto.Debit.Currency, dto.Debit.Debit), false);
            foreach (var credit in dto.Credits)
            {
              await UpdateElasticAccountBalance(scope, credit.AccountId, Money.Create(credit.Currency, credit.Credit), true);
            }
            return true;
          }
          return false;
        }
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return false;
      }
    }
    private async Task<bool> HandleMessageListCrListDr(ListCrListDr dto)
    {
      try
      {
        logger.LogInformation("Handling data from Ledger CrDr Que");
        using (var scope = Services.CreateScope())
        {
          var ledgerService = scope.ServiceProvider.GetRequiredService<ILedgerServices>();
          var res = await ledgerService.Transact(dto.Debits, dto.Credits, dto.UserId);
          if (res.Status)
          {
            foreach (var debit in dto.Debits)
            {
              await UpdateElasticAccountBalance(scope, debit.AccountId, Money.Create(debit.Currency, debit.Debit), false);
            }
            foreach (var credit in dto.Credits)
            {
              await UpdateElasticAccountBalance(scope, credit.AccountId, Money.Create(credit.Currency, credit.Credit), true);
            }
            return true;
          }
          return false;
        }
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return false;
      }
    }
    #endregion
    private async Task<bool> UpdateElasticAccountBalance(IServiceScope scope, Guid accId, Money amount, bool isCredit)
    {
      try
      {
        throw new NotImplementedException();
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return false;
      }

    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
      await RegisterCrDr();
      //await RegisterCrListDr();
      //await RegisterListCrDr();
      //await RegisterListCrListDr();
      return;
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
      connection.Close();
      return Task.CompletedTask;
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
      channel.Close();
      connection.Close();

    }
  }
}
