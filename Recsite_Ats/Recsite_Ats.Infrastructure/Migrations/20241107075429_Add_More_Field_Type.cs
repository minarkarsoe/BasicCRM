using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_More_Field_Type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                keyValue: 7,
                column: "FieldTypeName",
                value: "Decimal");

            migrationBuilder.InsertData(
                table: "FieldTypes",
                columns: new[] { "Id", "FieldTypeName" },
                values: new object[] { 8, "Image" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 7, 14, 54, 28, 627, DateTimeKind.Local).AddTicks(5932));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FieldTypes",
                keyColumn: "Id",
                keyValue: 8);

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

            migrationBuilder.UpdateData(
                table: "FieldTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "FieldTypeName",
                value: "Image");

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 7, 14, 53, 5, 694, DateTimeKind.Local).AddTicks(7574));
        }
    }
}
