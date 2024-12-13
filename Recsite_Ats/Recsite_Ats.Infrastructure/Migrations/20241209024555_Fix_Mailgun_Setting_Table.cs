using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Mailgun_Setting_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AccountId",
                table: "MailGunSettings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 12, 9, 9, 45, 54, 230, DateTimeKind.Local).AddTicks(5949), new DateTime(2024, 12, 9, 9, 45, 54, 230, DateTimeKind.Local).AddTicks(5950) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "ba26353b-18d5-4b5b-bfe1-a15fa2c3ef48", new DateTime(2024, 12, 9, 9, 45, 54, 230, DateTimeKind.Local).AddTicks(6044), new DateTime(2024, 12, 9, 9, 45, 54, 230, DateTimeKind.Local).AddTicks(6045), "AQAAAAIAAYagAAAAEO2/FoeOhs8MmkOUgeooue4k/dYSHTvzh+82Gyje+JSSG62zH17TDEsUXW1shQ8L5w==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 12, 9, 9, 45, 54, 230, DateTimeKind.Local).AddTicks(5935));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "MailGunSettings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 12, 6, 11, 0, 0, 162, DateTimeKind.Local).AddTicks(1852), new DateTime(2024, 12, 6, 11, 0, 0, 162, DateTimeKind.Local).AddTicks(1853) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "965099bf-2c60-4364-bd95-d9a8b7237870", new DateTime(2024, 12, 6, 11, 0, 0, 162, DateTimeKind.Local).AddTicks(1967), new DateTime(2024, 12, 6, 11, 0, 0, 162, DateTimeKind.Local).AddTicks(1968), "AQAAAAIAAYagAAAAEDuGH7N05yM4353f+qM5c/bmTNZBHvUXnQv2XTTxzTGH1Q+Jey7gX4YeLzWJsa9WgA==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 12, 6, 11, 0, 0, 162, DateTimeKind.Local).AddTicks(1836));
        }
    }
}
