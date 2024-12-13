using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Mailgun_Setting_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MailGunSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DomainName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApiKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailGunSettings", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailGunSettings");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 12, 4, 13, 12, 10, 505, DateTimeKind.Local).AddTicks(555), new DateTime(2024, 12, 4, 13, 12, 10, 505, DateTimeKind.Local).AddTicks(556) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "9563abb8-905e-4ff9-b375-f71a11aaf508", new DateTime(2024, 12, 4, 13, 12, 10, 505, DateTimeKind.Local).AddTicks(686), new DateTime(2024, 12, 4, 13, 12, 10, 505, DateTimeKind.Local).AddTicks(687), "AQAAAAIAAYagAAAAEKMIoL54JyZdk4iw5dGzfAckJNtIxGYg39OTstHCh0h+cEc3pwhJhV4Jrz6ek9mWTQ==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 12, 4, 13, 12, 10, 505, DateTimeKind.Local).AddTicks(533));
        }
    }
}
