﻿// <auto-generated />
using System;
using EngineeringThesis.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EngineeringThesis.Core.Migrations
{
    [DbContext(typeof(EngineeringThesisContext))]
    partial class EngineeringThesisContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<int>("StreetNumber")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SuiteNumber")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ZipCode")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CustomerTypeId");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BankAccountNumber = "154987526365212554788",
                            City = "Cieszyn",
                            CustomerTypeId = 1,
                            NIP = "1234567890",
                            Name = "Mateusz Polok",
                            PhoneNumber = "668055060",
                            REGON = "123456789",
                            Street = "Filasiewicza",
                            StreetNumber = 48,
                            ZipCode = "43-400"
                        });
                });

            modelBuilder.Entity("EngineeringThesis.Core.Models.CustomerType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
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

                    b.Property<string>("VATSum")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("InvoiceId");

                    b.ToTable("InvoiceItems");
                });

            modelBuilder.Entity("EngineeringThesis.Core.Models.PaymentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
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
