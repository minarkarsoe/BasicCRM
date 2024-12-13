using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_Job_Location_And_Categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 19, 12, 24, 44, 619, DateTimeKind.Local).AddTicks(5218), new DateTime(2024, 11, 19, 12, 24, 44, 619, DateTimeKind.Local).AddTicks(5219) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "30c6e0e7-35d1-4812-90cc-f4c6c35141e2", new DateTime(2024, 11, 19, 12, 24, 44, 619, DateTimeKind.Local).AddTicks(5333), new DateTime(2024, 11, 19, 12, 24, 44, 619, DateTimeKind.Local).AddTicks(5333), "AQAAAAIAAYagAAAAECgbY0C2+Rrm8xo4y5ITgvzwrZ+0RQI7bkUWHaGODM/TCXK//UZ62Ym8iLc8H9sLfA==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 19, 12, 24, 44, 619, DateTimeKind.Local).AddTicks(5204));
        }
    }
}
