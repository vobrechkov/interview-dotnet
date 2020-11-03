using BankingApi.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace BankingApi.Data
{
    public class BankingContext : DbContext
    {
        public BankingContext(DbContextOptions<BankingContext> options) : base(options)
        { }

        public DbSet<BankAccountNumber> BankAccountNumbers { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Institution> Institutions { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Transfer> Transfers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccountNumber>()
                .HasData(new BankAccountNumber() { LastNumber = 1 });

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.SourceTransaction)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.DestinationTransaction)
                .WithOne()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
