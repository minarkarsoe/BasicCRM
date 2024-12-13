using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_More_Field_Types : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 7, 14, 56, 47, 500, DateTimeKind.Local).AddTicks(9224), new DateTime(2024, 11, 7, 14, 56, 47, 500, DateTimeKind.Local).AddTicks(9225) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "a6535966-6eab-4167-a044-dbb57b77dd9b", new DateTime(2024, 11, 7, 14, 56, 47, 500, DateTimeKind.Local).AddTicks(9334), new DateTime(2024, 11, 7, 14, 56, 47, 500, DateTimeKind.Local).AddTicks(9335), "AQAAAAIAAYagAAAAEDceqvshOxlVHX3SQSTD7DlNAuA1Zj7yTsTHs3rVcWg3pus3iNep4ElyYTlhrdywmA==" });

            migrationBuilder.UpdateData(
                table: "FieldTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "FieldTypeName",
                value: "DateTime");

            migrationBuilder.InsertData(
                table: "FieldTypes",
                columns: new[] { "Id", "FieldTypeName" },
                values: new object[] { 9, "Image" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 7, 14, 56, 47, 500, DateTimeKind.Local).AddTicks(9205));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FieldTypes",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 7, 14, 54, 28, 627, DateTimeKind.Local).AddTicks(5949), new DateTime(2024, 11, 7, 14, 54, 28, 627, DateTimeKind.Local).AddTicks(5950) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "b1f8ba7c-0306-44ed-985c-61087657b0e2", new DateTime(2024, 11, 7, 14, 54, 28, 627, DateTimeKind.Local).AddTicks(6076), new DateTime(2024, 11, 7, 14, 54, 28, 627, DateTimeKind.Local).AddTicks(6078), "AQAAAAIAAYagAAAAENULmTBNZX9cFSYMF7jIcIr2sYcmuZi1IsL3Mui80H9BQb15/vZa6SBMAOF8Mlgffg==" });

            migrationBuilder.UpdateData(
                table: "FieldTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "FieldTypeName",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 7, 14, 54, 28, 627, DateTimeKind.Local).AddTicks(5932));
        }
    }
}
