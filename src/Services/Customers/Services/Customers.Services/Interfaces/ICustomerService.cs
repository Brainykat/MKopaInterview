using Customers.Domain.Entities;
using Customers.ViewsAndDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customers.Services.Interfaces
{
  public interface ICustomerService
  {
    Task<int> Add(CustomerDto dto);
    Task<int> Delete(Guid id);
    Task<Customer> GetCustomer(Guid id);
    Task<List<Customer>> GetCustomers();
    Task<int> Update(Guid id, CustomerDto dto);
  }
}
