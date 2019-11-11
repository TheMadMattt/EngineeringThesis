using System;
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = AppDomain.CurrentDomain.BaseDirectory;
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
                    NIP = "123-456-78-90", 
                    REGON = "123-45-67-89",
                    BankAccountNumber = "15498752636521255478888888",
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
                    NIP = "098-765-43-21",
                    REGON = "987-65-43-21",
                    BankAccountNumber = "15498752632341255478888888",
                    CustomerTypeId = 1
                },
                new Customer
                {
                    Id = 3,
                    Name = "Bartłomiej Blicharski",
                    ZipCode = "50-348",
                    City = "Wrocław",
                    Street = "Sienkiewicza",
                    StreetNumber = "102/1",
                    PhoneNumber = "223656089",
                    NIP = "108-365-73-21",
                    REGON = "923-65-46-21",
                    BankAccountNumber = "32493456636521255478888888",
                    CustomerTypeId = 2
                },
                new Customer
                {
                    Id = 4,
                    Name = "Sebastian Stanclik",
                    ZipCode = "50-348",
                    City = "Wrocław",
                    Street = "Sienkiewicza",
                    StreetNumber = "102/1",
                    PhoneNumber = "623256780",
                    NIP = "568-789-40-01",
                    REGON = "923-65-45-21",
                    BankAccountNumber = "38493452632341255478888888",
                    CustomerTypeId = 1
                }
            );

            modelBuilder.Entity<Invoice>().HasData(
                new Invoice
                {
                    Id=1,
                    InvoiceNumber = "01/2019",
                    InvoiceDate = DateTime.Today,
                    ContractorId = 2,
                    SellerId = 1,
                    PaymentTypeId = 2,
                    PaymentDeadline = new DateTime(2020,11,11),
                    PaymentDate = new DateTime(2020,10,19)
                },
                new Invoice
                {
                    Id=2,
                    InvoiceNumber = "02/2019",
                    InvoiceDate = new DateTime(2019,09,14),
                    ContractorId = 4,
                    SellerId = 3,
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
                    NetPrice = "1500,00",
                    Amount = 1,
                    VAT = 23,
                    VATSum = "345,00",
                    NetSum = "1500,00",
                    GrossSum = "1845"
                },
                new InvoiceItem
                {
                    Id = 1,
                    InvoiceId = 1,
                    Name = "Tablet",
                    Unit = "szt",
                    NetPrice = "2000,00",
                    Amount = 1,
                    VAT = 23,
                    VATSum = "460,00",
                    NetSum = "2000,00",
                    GrossSum = "2460,00"
                });
        }
    }
}
