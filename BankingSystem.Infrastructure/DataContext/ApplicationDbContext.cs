using BankingSystem.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace BankingSystem.Infrastructure.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {}

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<AtmCard> AtmCards { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Property(e => e.DateRegistered).HasColumnType("timestamp with time zone");

            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Account>().Property(a => a.Id).IsRequired();
            modelBuilder.Entity<Account>().Property(a => a.Balance).HasPrecision(18, 2);

            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            modelBuilder.Entity<Transaction>().Property(t => t.Id).IsRequired();
            modelBuilder.Entity<Transaction>().Property(t => t.Amount).HasPrecision(18, 2);

            modelBuilder.Entity<AtmCard>().HasKey(a => a.Id);
            modelBuilder.Entity<AtmCard>().Property(a => a.Id).IsRequired();

            modelBuilder.Entity<Contact>().HasKey(c => c.Id);
            modelBuilder.Entity<Contact>().Property(c => c.Id).IsRequired();
        }
    }
}
