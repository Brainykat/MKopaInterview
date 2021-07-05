using Customers.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customers.Domain.Interfaces
{
  public interface ICustomerRepo
  {
    Task AddCustomer(Customer customer);
    Task<Customer> GetCustomer(Guid id);
    Task<List<Customer>> GetCustomers();
    Task<Customer> GetCustomerTracked(Guid id);
    Task UpdateCustomer(Customer customer);
  }
}
