using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EngineeringThesis.Core.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: false),
                    ZipCode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    StreetNumber = table.Column<int>(nullable: false),
                    SuiteNumber = table.Column<int>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    NIP = table.Column<string>(nullable: true),
                    REGON = table.Column<string>(nullable: true),
                    BankAccountNumber = table.Column<string>(nullable: true),
                    CustomerTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_CustomerTypes_CustomerTypeId",
                        column: x => x.CustomerTypeId,
                        principalTable: "CustomerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceNumber = table.Column<string>(nullable: false),
                    InvoiceDate = table.Column<DateTime>(nullable: false),
                    ContractorId = table.Column<int>(nullable: false),
                    SellerId = table.Column<int>(nullable: false),
                    PaymentTypeId = table.Column<int>(nullable: false),
                    PaymentDeadline = table.Column<DateTime>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoices_Customers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InvoiceId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    PKWiU = table.Column<string>(nullable: true),
                    Unit = table.Column<string>(nullable: true),
                    NetPrice = table.Column<string>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    VATSum = table.Column<int>(nullable: false),
                    NetSum = table.Column<string>(nullable: true),
                    GrossSum = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceItems_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Kontrahent" });

            migrationBuilder.InsertData(
                table: "CustomerTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Sprzedawca" });

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Gotówka" });

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Przelew" });

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 3, "Karta" });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "BankAccountNumber", "City", "CustomerTypeId", "NIP", "Name", "PhoneNumber", "REGON", "Street", "StreetNumber", "SuiteNumber", "ZipCode" },
                values: new object[] { 1, "154987526365212554788", "Cieszyn", 1, "1234567890", "Mateusz Polok", "668055060", "123456789", "Filasiewicza", 48, null, "43-400" });

            migrationBuilder.InsertData(
                table: "Invoices",
                columns: new[] { "Id", "Comments", "ContractorId", "InvoiceDate", "InvoiceNumber", "PaymentDeadline", "PaymentTypeId", "SellerId" },
                values: new object[] { 1, null, 1, new DateTime(2019, 10, 10, 0, 0, 0, 0, DateTimeKind.Local), "01/2019", new DateTime(2019, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1 });

            migrationBuilder.InsertData(
                table: "InvoiceItems",
                columns: new[] { "Id", "Amount", "Comments", "GrossSum", "InvoiceId", "Name", "NetPrice", "NetSum", "PKWiU", "Unit", "VATSum" },
                values: new object[] { 2, 1, null, "1845", 1, "Smartfon", "1500", "1500", null, "szt", 23 });

            migrationBuilder.InsertData(
                table: "InvoiceItems",
                columns: new[] { "Id", "Amount", "Comments", "GrossSum", "InvoiceId", "Name", "NetPrice", "NetSum", "PKWiU", "Unit", "VATSum" },
                values: new object[] { 1, 1, null, "2460", 1, "Tablet", "2000", "2000", null, "szt", 23 });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CustomerTypeId",
                table: "Customers",
                column: "CustomerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItems_InvoiceId",
                table: "InvoiceItems",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ContractorId",
                table: "Invoices",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_PaymentTypeId",
                table: "Invoices",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_SellerId",
                table: "Invoices",
                column: "SellerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceItems");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "CustomerTypes");
        }
    }
}
