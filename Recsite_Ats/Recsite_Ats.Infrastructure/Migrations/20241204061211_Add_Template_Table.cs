using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Template_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationModules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Template = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 12, 4, 13, 12, 10, 505, DateTimeKind.Local).AddTicks(555), new DateTime(2024, 12, 4, 13, 12, 10, 505, DateTimeKind.Local).AddTicks(556) });

            migrationBuilder.InsertData(
                table: "ApplicationModules",
                columns: new[] { "Id", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, true, "Contacts" },
                    { 2, true, "Jobs" },
                    { 3, true, "Candidates" },
                    { 4, true, "Companies" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "9563abb8-905e-4ff9-b375-f71a11aaf508", new DateTime(2024, 12, 4, 13, 12, 10, 505, DateTimeKind.Local).AddTicks(686), new DateTime(2024, 12, 4, 13, 12, 10, 505, DateTimeKind.Local).AddTicks(687), "AQAAAAIAAYagAAAAEKMIoL54JyZdk4iw5dGzfAckJNtIxGYg39OTstHCh0h+cEc3pwhJhV4Jrz6ek9mWTQ==" });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "Id", "AccountId", "Description", "IsDefault", "ModuleId", "Name", "Template" },
                values: new object[] { 1, 0, "This is Contact Default Template", true, 1, "Contact Default Template", "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <title>Recovery Code</title>\r\n</head>\r\n<body>\r\n    <h1>This is Contact Default Template</h1>\r\n    <p>Thank you for using our service.</p>\r\n</body>\r\n</html>" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 12, 4, 13, 12, 10, 505, DateTimeKind.Local).AddTicks(533));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationModules");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 19, 12, 34, 43, 544, DateTimeKind.Local).AddTicks(6077), new DateTime(2024, 11, 19, 12, 34, 43, 544, DateTimeKind.Local).AddTicks(6080) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "fb6c8880-4b63-44c3-8c29-19800fc8e458", new DateTime(2024, 11, 19, 12, 34, 43, 544, DateTimeKind.Local).AddTicks(6184), new DateTime(2024, 11, 19, 12, 34, 43, 544, DateTimeKind.Local).AddTicks(6184), "AQAAAAIAAYagAAAAEEbarKEbW1uCuAFsiSwqAupykRinS37cbXmIUUzGv9ocmJjp/KS1xXHPDHVZnrtH6A==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 19, 12, 34, 43, 544, DateTimeKind.Local).AddTicks(6060));
        }
    }
}
