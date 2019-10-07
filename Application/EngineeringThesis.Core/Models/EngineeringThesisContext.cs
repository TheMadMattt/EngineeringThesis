using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EngineeringThesis.Core.Models
{
    public class EngineeringThesisContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }

        public EngineeringThesisContext() : base()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = System.AppDomain.CurrentDomain.BaseDirectory;
            connectionString += "\\Invoices.db";
            optionsBuilder
                .UseSqlite($@"Data Source={connectionString}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Invoice>().ToTable("Invoices");
            modelBuilder.Entity<InvoiceItem>().ToTable("InvoiceItems");
            modelBuilder.Entity<CustomerType>().ToTable("CustomerTypes");
            modelBuilder.Entity<PaymentType>().ToTable("PaymentTypes");

            modelBuilder.Entity<CustomerType>().HasData(
                new CustomerType { Id = 1, Name = "Kontrahent"},
                new CustomerType { Id = 2, Name = "Sprzedawca"}
            );

            modelBuilder.Entity<PaymentType>().HasData(
                new PaymentType { Id = 1, Name = "Gotówka" },
                new PaymentType { Id = 2, Name = "Przelew" },
                new PaymentType { Id = 3, Name = "Karta" }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1, 
                    Name = "Mateusz Polok", 
                    ZipCode = "43-400", 
                    City = "Cieszyn", 
                    Street = "Filasiewicza", 
                    StreetNumber = 48,
                    PhoneNumber = "668055060", 
                    NIP = "1234567890", 
                    REGON = "123456789",
                    BankAccountNumber = "154987526365212554788", 
                    CustomerTypeId = 1
                }
            );
        }
    }
}
