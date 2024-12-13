using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_More_Role : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 7, 15, 49, 5, 880, DateTimeKind.Local).AddTicks(3654), new DateTime(2024, 11, 7, 15, 49, 5, 880, DateTimeKind.Local).AddTicks(3655) });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { 2, null, "Recruiter", "RECRUITER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "91de1c30-e3fc-46fd-bf31-b1236cd6a00b", new DateTime(2024, 11, 7, 15, 49, 5, 880, DateTimeKind.Local).AddTicks(3766), new DateTime(2024, 11, 7, 15, 49, 5, 880, DateTimeKind.Local).AddTicks(3767), "AQAAAAIAAYagAAAAEJfEnNoMrHXz66gjl8Vqzk4AUnRWTtFyzU3jRvXzvr0uixKkw4TY5KHK3CMnEkr37Q==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 7, 15, 49, 5, 880, DateTimeKind.Local).AddTicks(3638));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2);

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
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 7, 14, 56, 47, 500, DateTimeKind.Local).AddTicks(9205));
        }
    }
}
