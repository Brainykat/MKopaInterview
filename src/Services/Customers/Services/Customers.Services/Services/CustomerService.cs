using Common.Shared.Services;
using Customers.Data.Repositories;
using Customers.Domain.Entities;
using Customers.Services.Interfaces;
using Customers.ViewsAndDtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Services.Services
{
  public class CustomerService : ICustomerService
  {
    private readonly CustomerRepo repo;
    private readonly ILogger<CustomerService> logger;
    private readonly ICustomerEventProducer eventProducer;
    public CustomerService(CustomerRepo repo, ILogger<CustomerService> logger, ICustomerEventProducer eventProducer)
    {
      this.repo = repo ?? throw new ArgumentNullException(nameof(repo));
      this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
      this.eventProducer = eventProducer;
    }
    public async Task<List<Customer>> GetCustomers() => await repo.GetCustomers();
    public async Task<Customer> GetCustomer(Guid id) => await repo.GetCustomer(id);

    public async Task<int> Add(CustomerDto dto)
    {
      try
      {
        var customer = Customer.Create(dto.FirstName, dto.LastName, dto.DateOfBirth, dto.IdNumber, dto.PhoneNumber);
        await repo.AddCustomer(customer);
        //UNDONE: Raise Account opening event
        eventProducer.RaisePaymentEvent(customer);
        return 201;
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod());
        return 500;
      }

    }
    public async Task<int> Update(Guid id, CustomerDto dto)
    {
      try
      {
        var customer = await repo.GetCustomerTracked(id);
        if (customer != null)
        {
          customer.FirstName = dto.FirstName;
          customer.LastName = dto.LastName;
          customer.IdNumber = dto.IdNumber;
          customer.PhoneNumber = dto.PhoneNumber;
          customer.DateOfBirth = dto.DateOfBirth;
          await repo.UpdateCustomer(customer);
          return 202;
        }
        return 404;
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod()); return 500;
      }
    }
    public async Task<int> Delete(Guid id)
    {
      try
      {
        var customer = await repo.GetCustomerTracked(id);
        if (customer != null)
        {
          customer.Status = 9;
          await repo.UpdateCustomer(customer);
          return 204;
        }
        return 404;
      }
      catch (Exception ex)
      {
        LogHelper.LogError(logger, ex, MethodBase.GetCurrentMethod()); return 500;
      }
    }
  }
}
