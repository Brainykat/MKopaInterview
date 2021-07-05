using Customers.Data.Configurations;
using Customers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Customers.Data
{
  public class CustomerContext : DbContext
  {
    public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
    {
    }
    public DbSet<Customer> Customers { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfiguration(new CustomerConfig());
      //InitialAccountsSeed.InitialSeed(modelBuilder);
    }
  }
}
