using Customers.Domain.Entities;
using Customers.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customers.Data.Repositories
{


  public class CustomerRepo : ICustomerRepo
  {
    private readonly CustomerContext context;
    public CustomerRepo(CustomerContext context)
    {
      this.context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<List<Customer>> GetCustomers() => await context.Customers.AsNoTracking().ToListAsync();
    public async Task<Customer> GetCustomer(Guid id) => await context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
    public async Task<Customer> GetCustomerTracked(Guid id) => await context.Customers.FirstOrDefaultAsync(c => c.Id == id);
    public async Task AddCustomer(Customer customer)
    {
      context.Customers.Add(customer);
      await context.SaveChangesAsync();
    }
    public async Task UpdateCustomer(Customer customer)
    {
      context.Customers.Update(customer);
      await context.SaveChangesAsync();
    }
  }
}
