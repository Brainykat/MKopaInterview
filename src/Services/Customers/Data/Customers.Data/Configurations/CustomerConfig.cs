using Customers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customers.Data.Configurations
{
  public class CustomerConfig : IEntityTypeConfiguration<Customer>
  {
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
      builder.HasKey(prop => prop.Id);
      builder.Property(e => e.FirstName)
          .IsRequired()
          .HasMaxLength(150);
      builder.Property(e => e.LastName)
         .HasMaxLength(150);
      builder.Property(e => e.IdNumber)
         .HasMaxLength(50);
      builder.HasIndex(b => b.IdNumber).IsUnique();
      builder.Property(e => e.PhoneNumber)
         .IsRequired()
         .HasMaxLength(15);
    }
  }
}
