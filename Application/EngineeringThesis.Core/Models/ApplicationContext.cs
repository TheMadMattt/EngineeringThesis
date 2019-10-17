using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace EngineeringThesis.Core.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<CustomerType> CustomerTypes { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }

        public ApplicationContext() : base()
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
                    StreetNumber = "48",
                    PhoneNumber = "668055060", 
                    NIP = "1234567890", 
                    REGON = "123456789",
                    BankAccountNumber = "154987526365212554788",
                    CustomerTypeId = 2
                },
                new Customer
                {
                    Id = 2,
                    Name = "Bartosz Prokopowicz",
                    ZipCode = "50-348",
                    City = "Wrocław",
                    Street = "Sienkiewicza",
                    StreetNumber = "102/1",
                    PhoneNumber = "123456789",
                    NIP = "0987654321",
                    REGON = "987654321",
                    BankAccountNumber = "758492113124142414103",
                    CustomerTypeId = 1
                }
            );

            modelBuilder.Entity<Invoice>().HasData(
                new Invoice
                {
                    Id=1,
                    InvoiceNumber = "01/2019",
                    InvoiceDate = DateTime.Today,
                    ContractorId = 1,
                    SellerId = 2,
                    PaymentTypeId = 2,
                    PaymentDeadline = new DateTime(2019,11,1)
                },
                new Invoice
                {
                    Id=2,
                    InvoiceNumber = "02/2019",
                    InvoiceDate = new DateTime(2019,09,14),
                    ContractorId = 1,
                    SellerId = 2,
                    PaymentTypeId = 3,
                    PaymentDeadline = new DateTime(2020,05,14)
                }
            );

            modelBuilder.Entity<InvoiceItem>().HasData(
                new InvoiceItem
                {
                    Id = 2,
                    InvoiceId = 1,
                    Name = "Smartfon",
                    Unit = "szt",
                    NetPrice = "1500",
                    Amount = 1,
                    VATSum = 23,
                    NetSum = "1500",
                    GrossSum = "1845"
                },
                new InvoiceItem
                {
                    Id = 1,
                    InvoiceId = 1,
                    Name = "Tablet",
                    Unit = "szt",
                    NetPrice = "2000",
                    Amount = 1,
                    VATSum = 23,
                    NetSum = "2000",
                    GrossSum = "2460"
                });
        }
    }
}
