using Microsoft.EntityFrameworkCore;
using System;

namespace Accounts.Data
{
  public class AccountsContext : DbContext
  {
    public AccountsContext(DbContextOptions<AccountsContext> options) : base(options)
    {
    }
    public DbSet<ChartOfAccount> ChartOfAccounts { get; set; }
    public DbSet<Account> Accounts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
      modelBuilder.ApplyConfiguration(new AccountConfig());
      modelBuilder.ApplyConfiguration(new ChartOfAccountConfig());
      //InitialAccountsSeed.InitialSeed(modelBuilder);
    }
  }
}
