using Loaning.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Loaning.Data
{
  public class LoaningContext : DbContext
  {
    public LoaningContext(DbContextOptions<LoaningContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfiguration(new ProductConfiguration());
      modelBuilder.ApplyConfiguration(new LoanConfiguration());
    }
  }
}
