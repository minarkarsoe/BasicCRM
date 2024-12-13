using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Billing_Info : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillingInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VATNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingInformation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingInformation_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 12, 12, 2, 45, 131, DateTimeKind.Local).AddTicks(3373), new DateTime(2024, 11, 12, 12, 2, 45, 131, DateTimeKind.Local).AddTicks(3374) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "d26dbe47-f07b-43c7-8ac1-eb9048171f81", new DateTime(2024, 11, 12, 12, 2, 45, 131, DateTimeKind.Local).AddTicks(3467), new DateTime(2024, 11, 12, 12, 2, 45, 131, DateTimeKind.Local).AddTicks(3468), "AQAAAAIAAYagAAAAEOMOxyHbHq0Rizu4/ZtCa/1VE9BJ2Bzn/C4C3SXBRkPn/9ICYQ7MO1TIp4eQhBt+Cg==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 12, 12, 2, 45, 131, DateTimeKind.Local).AddTicks(3360));

            migrationBuilder.CreateIndex(
                name: "IX_BillingInformation_AccountId",
                table: "BillingInformation",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillingInformation");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 11, 14, 45, 50, 252, DateTimeKind.Local).AddTicks(7336), new DateTime(2024, 11, 11, 14, 45, 50, 252, DateTimeKind.Local).AddTicks(7336) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "66aa5bda-b8c9-44be-9bf9-23eccbd71510", new DateTime(2024, 11, 11, 14, 45, 50, 252, DateTimeKind.Local).AddTicks(7438), new DateTime(2024, 11, 11, 14, 45, 50, 252, DateTimeKind.Local).AddTicks(7439), "AQAAAAIAAYagAAAAEPbgGiek4607rgS6k7bOvz28S4MbtOM9PlyKBlt/iYSaaw0/u6EBYo679LljttqWRA==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 11, 14, 45, 50, 252, DateTimeKind.Local).AddTicks(7320));
        }
    }
}
