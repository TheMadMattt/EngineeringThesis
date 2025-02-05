﻿// <auto-generated />
using System;
using EngineeringThesis.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EngineeringThesis.Core.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20191113185222_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0");

            modelBuilder.Entity("EngineeringThesis.Core.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BankAccountNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .HasColumnType("TEXT");

                    b.Property<string>("Comments")
                        .HasColumnType("TEXT");

                    b.Property<int>("CustomerTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("NIP")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("REGON")
                        .HasColumnType("TEXT");

                    b.Property<string>("Street")
                        .HasColumnType("TEXT");

                    b.Property<string>("StreetNumber")
                        .HasColumnType("TEXT");

                    b.Property<string>("ZipCode")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CustomerTypeId");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BankAccountNumber = "15498752636521255478888888",
                            City = "Cieszyn",
                            CustomerTypeId = 2,
                            NIP = "123-456-78-90",
                            Name = "Mateusz Polok",
                            PhoneNumber = "668055060",
                            REGON = "123-45-67-89",
                            Street = "Filasiewicza",
                            StreetNumber = "48",
                            ZipCode = "43-400"
                        },
                        new
                        {
                            Id = 2,
                            BankAccountNumber = "15498752632341255478888888",
                            City = "Wrocław",
                            CustomerTypeId = 1,
                            NIP = "098-765-43-21",
                            Name = "Bartosz Prokopowicz",
                            PhoneNumber = "123456789",
                            REGON = "987-65-43-21",
                            Street = "Sienkiewicza",
                            StreetNumber = "102/1",
                            ZipCode = "50-348"
                        },
                        new
                        {
                            Id = 3,
                            BankAccountNumber = "32493456636521255478888888",
                            City = "Wrocław",
                            CustomerTypeId = 2,
                            NIP = "108-365-73-21",
                            Name = "Bartłomiej Blicharski",
                            PhoneNumber = "223656089",
                            REGON = "923-65-46-21",
                            Street = "Sienkiewicza",
                            StreetNumber = "102/1",
                            ZipCode = "50-348"
                        },
                        new
                        {
                            Id = 4,
                            BankAccountNumber = "38493452632341255478888888",
                            City = "Wrocław",
                            CustomerTypeId = 1,
                            NIP = "568-789-40-01",
                            Name = "Sebastian Stanclik",
                            PhoneNumber = "623256780",
                            REGON = "923-65-45-21",
                            Street = "Sienkiewicza",
                            StreetNumber = "102/1",
                            ZipCode = "50-348"
                        });
                });

            modelBuilder.Entity("EngineeringThesis.Core.Models.CustomerType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CustomerTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Kontrahent"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Sprzedawca"
                        });
                });

            modelBuilder.Entity("EngineeringThesis.Core.Models.Invoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comments")
                        .HasColumnType("TEXT");

                    b.Property<int>("ContractorId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("InvoiceDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("InvoiceNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsProformaInvoice")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("PaymentDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PaymentDeadline")
                        .HasColumnType("TEXT");

                    b.Property<int>("PaymentTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SellerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ContractorId");

                    b.HasIndex("PaymentTypeId");

                    b.HasIndex("SellerId");

                    b.ToTable("Invoices");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ContractorId = 2,
                            InvoiceDate = new DateTime(2019, 11, 13, 0, 0, 0, 0, DateTimeKind.Local),
                            InvoiceNumber = "1/2019",
                            IsProformaInvoice = false,
                            PaymentDate = new DateTime(2020, 10, 19, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PaymentDeadline = new DateTime(2020, 11, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PaymentTypeId = 2,
                            SellerId = 1
                        },
                        new
                        {
                            Id = 2,
                            ContractorId = 4,
                            InvoiceDate = new DateTime(2019, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            InvoiceNumber = "2/2019",
                            IsProformaInvoice = false,
                            PaymentDeadline = new DateTime(2020, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            PaymentTypeId = 3,
                            SellerId = 3
                        });
                });

            modelBuilder.Entity("EngineeringThesis.Core.Models.InvoiceItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Amount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comments")
                        .HasColumnType("TEXT");

                    b.Property<string>("GrossSum")
                        .HasColumnType("TEXT");

                    b.Property<int>("InvoiceId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NetPrice")
                        .HasColumnType("TEXT");

                    b.Property<string>("NetSum")
                        .HasColumnType("TEXT");

                    b.Property<string>("PKWiU")
                        .HasColumnType("TEXT");

                    b.Property<string>("Unit")
                        .HasColumnType("TEXT");

                    b.Property<int>("VAT")
                        .HasColumnType("INTEGER");

                    b.Property<string>("VATSum")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoiceItems");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Amount = 1,
                            GrossSum = "1845",
                            InvoiceId = 1,
                            Name = "Smartfon",
                            NetPrice = "1500,00",
                            NetSum = "1500,00",
                            Unit = "szt",
                            VAT = 23,
                            VATSum = "345,00"
                        },
                        new
                        {
                            Id = 1,
                            Amount = 1,
                            GrossSum = "2460,00",
                            InvoiceId = 1,
                            Name = "Tablet",
                            NetPrice = "2000,00",
                            NetSum = "2000,00",
                            Unit = "szt",
                            VAT = 23,
                            VATSum = "460,00"
                        });
                });

            modelBuilder.Entity("EngineeringThesis.Core.Models.LastInvoiceNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("InvoiceNumber")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LastInvoiceNumbers");
                });

            modelBuilder.Entity("EngineeringThesis.Core.Models.PaymentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("PaymentTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Gotówka"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Przelew"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Karta"
                        });
                });

            modelBuilder.Entity("EngineeringThesis.Core.Models.Customer", b =>
                {
                    b.HasOne("EngineeringThesis.Core.Models.CustomerType", "CustomerType")
                        .WithMany()
                        .HasForeignKey("CustomerTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EngineeringThesis.Core.Models.Invoice", b =>
                {
                    b.HasOne("EngineeringThesis.Core.Models.Customer", "Contractor")
                        .WithMany()
                        .HasForeignKey("ContractorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EngineeringThesis.Core.Models.PaymentType", "PaymentType")
                        .WithMany()
                        .HasForeignKey("PaymentTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EngineeringThesis.Core.Models.Customer", "Seller")
                        .WithMany()
                        .HasForeignKey("SellerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EngineeringThesis.Core.Models.InvoiceItem", b =>
                {
                    b.HasOne("EngineeringThesis.Core.Models.Invoice", "Invoice")
                        .WithMany("InvoiceItems")
                        .HasForeignKey("InvoiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
