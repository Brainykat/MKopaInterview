using Ledgers.Data.Configurations;
using Ledgers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ledgers.Data
{
  public class LedgerContext : DbContext
  {
    public LedgerContext(DbContextOptions<LedgerContext> options) : base(options)
    {
    }
    public DbSet<Wallet> Wallets { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfiguration(new WalletConfiguration());
    }
  }
}
