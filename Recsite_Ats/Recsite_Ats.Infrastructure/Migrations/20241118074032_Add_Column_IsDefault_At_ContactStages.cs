using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Column_IsDefault_At_ContactStages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "ContactStages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 18, 14, 40, 31, 830, DateTimeKind.Local).AddTicks(9945), new DateTime(2024, 11, 18, 14, 40, 31, 830, DateTimeKind.Local).AddTicks(9945) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "7ee10a3b-87f8-4aaf-a18f-8b3e6f50a64a", new DateTime(2024, 11, 18, 14, 40, 31, 831, DateTimeKind.Local).AddTicks(43), new DateTime(2024, 11, 18, 14, 40, 31, 831, DateTimeKind.Local).AddTicks(44), "AQAAAAIAAYagAAAAEMSULSVuTTW2a5LqZL/Yc/X+iN++0827Oo//6IoramccpoKnm6LvDcRMK3NyvD2nZw==" });

            migrationBuilder.UpdateData(
                table: "ContactStages",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsDefault",
                value: false);

            migrationBuilder.UpdateData(
                table: "ContactStages",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsDefault",
                value: false);

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 18, 14, 40, 31, 830, DateTimeKind.Local).AddTicks(9933));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "ContactStages");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 18, 14, 25, 31, 284, DateTimeKind.Local).AddTicks(2628), new DateTime(2024, 11, 18, 14, 25, 31, 284, DateTimeKind.Local).AddTicks(2629) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "478227e5-aed1-43b4-b568-b8f3e618b152", new DateTime(2024, 11, 18, 14, 25, 31, 284, DateTimeKind.Local).AddTicks(2773), new DateTime(2024, 11, 18, 14, 25, 31, 284, DateTimeKind.Local).AddTicks(2774), "AQAAAAIAAYagAAAAEJaIe/zFFIGXC986rpG7DC1GPaIAnXNLmhvhtrHJWY3wj0XZgm6237bjPnzCAStsTg==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 18, 14, 25, 31, 284, DateTimeKind.Local).AddTicks(2612));
        }
    }
}
