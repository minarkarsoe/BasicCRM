using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Payment_Method : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CardNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiryYear = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMethods_Accounts_AccountId",
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
                values: new object[] { new DateTime(2024, 11, 12, 16, 54, 26, 302, DateTimeKind.Local).AddTicks(785), new DateTime(2024, 11, 12, 16, 54, 26, 302, DateTimeKind.Local).AddTicks(786) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "e4d2de92-bf28-4bf1-9ff4-2433b0ee37fa", new DateTime(2024, 11, 12, 16, 54, 26, 302, DateTimeKind.Local).AddTicks(888), new DateTime(2024, 11, 12, 16, 54, 26, 302, DateTimeKind.Local).AddTicks(889), "AQAAAAIAAYagAAAAEBxyns81hrLo1IkArnCeh4DmRSxDxTBKf1ETbx3BPuvaejB9QVA0aF+aqbhq1wO8Fw==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 12, 16, 54, 26, 302, DateTimeKind.Local).AddTicks(773));

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_AccountId",
                table: "PaymentMethods",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentMethods");

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
        }
    }
}
