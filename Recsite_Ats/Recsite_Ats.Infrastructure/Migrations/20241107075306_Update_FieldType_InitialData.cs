using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update_FieldType_InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 7, 14, 53, 5, 694, DateTimeKind.Local).AddTicks(7618), new DateTime(2024, 11, 7, 14, 53, 5, 694, DateTimeKind.Local).AddTicks(7619) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "5b7daaa0-091d-44d4-a4ee-af254a931baa", new DateTime(2024, 11, 7, 14, 53, 5, 694, DateTimeKind.Local).AddTicks(7707), new DateTime(2024, 11, 7, 14, 53, 5, 694, DateTimeKind.Local).AddTicks(7708), "AQAAAAIAAYagAAAAECuTbQvJcVD9TUyUN8327+yMbwbN1mEhTnsUNY/XVcN3p6fRpa4kEAxhwshOFM59eQ==" });

            migrationBuilder.InsertData(
                table: "FieldTypes",
                columns: new[] { "Id", "FieldTypeName" },
                values: new object[] { 7, "Image" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 7, 14, 53, 5, 694, DateTimeKind.Local).AddTicks(7574));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FieldTypes",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 7, 14, 36, 59, 499, DateTimeKind.Local).AddTicks(8073), new DateTime(2024, 11, 7, 14, 36, 59, 499, DateTimeKind.Local).AddTicks(8075) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "73ded9e2-f34b-41fc-9c58-0ab1efaf739a", new DateTime(2024, 11, 7, 14, 36, 59, 499, DateTimeKind.Local).AddTicks(8177), new DateTime(2024, 11, 7, 14, 36, 59, 499, DateTimeKind.Local).AddTicks(8178), "AQAAAAIAAYagAAAAEAv0FLcAn5BU5TuX4+xbXNfPT6IfbrdOu5dUyZi+bpnbSloqVGdcv0ln2WDUiZj1zg==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 7, 14, 36, 59, 499, DateTimeKind.Local).AddTicks(8016));
        }
    }
}
