using Ledgers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ledgers.Data.Configurations
{
  public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
  {
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
      builder.HasKey(prop => prop.Id);
      builder.Property(e => e.AccountId)
      .IsRequired();
      builder.Property(e => e.TxnTime)
      .IsRequired().HasDefaultValueSql("GetUtcDate()");
      builder.OwnsOne(i => i.Credit, f =>
      {
        f.Property(n => n.Currency).IsRequired().HasMaxLength(6);
        f.Property(n => n.Amount).IsRequired().HasColumnType("decimal(18,4)");
        f.Property(n => n.Time).IsRequired();
      });
      builder.OwnsOne(i => i.Debit, f =>
      {
        f.Property(n => n.Currency).IsRequired().HasMaxLength(6);
        f.Property(n => n.Amount).IsRequired().HasColumnType("decimal(18,4)");
        f.Property(n => n.Time).IsRequired();
      });
      builder.Property(e => e.TxnRefrence)
      .IsRequired();
      builder.Property(e => e.Narration)
      .IsRequired()
      .HasMaxLength(100);
      builder.Property(e => e.TransactingUser)
      .IsRequired();
      builder.Property(e => e.DateCreated).HasDefaultValueSql("GetUtcDate()");
    }
  }
}
