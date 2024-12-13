using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_CountryData_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISO_CODES = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryData", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryData");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 11, 11, 32, 23, 262, DateTimeKind.Local).AddTicks(5435), new DateTime(2024, 11, 11, 11, 32, 23, 262, DateTimeKind.Local).AddTicks(5436) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "0ec43191-4f5b-4381-a674-24ccf1b98774", new DateTime(2024, 11, 11, 11, 32, 23, 262, DateTimeKind.Local).AddTicks(5536), new DateTime(2024, 11, 11, 11, 32, 23, 262, DateTimeKind.Local).AddTicks(5536), "AQAAAAIAAYagAAAAEOPV7j+b7+ThjPxyctEsHDdNjVp8VCJX08nIVz/Gwnz41NKqPBUSCpWYQvxOWhCGrA==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 11, 11, 32, 23, 262, DateTimeKind.Local).AddTicks(5423));
        }
    }
}
