using Microsoft.EntityFrameworkCore;

namespace Sanlam.Banking.Data.EF
{
    public class BankDbContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("accounts");
            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Account>().Property(a => a.Balance).HasColumnName("balance");
        }
    }
}
