using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Document_Type_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCustomized = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 15, 16, 19, 32, 465, DateTimeKind.Local).AddTicks(5258), new DateTime(2024, 11, 15, 16, 19, 32, 465, DateTimeKind.Local).AddTicks(5258) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "86fecf66-23f6-4c3b-9846-0048e64459b4", new DateTime(2024, 11, 15, 16, 19, 32, 465, DateTimeKind.Local).AddTicks(5354), new DateTime(2024, 11, 15, 16, 19, 32, 465, DateTimeKind.Local).AddTicks(5355), "AQAAAAIAAYagAAAAEDf49/prw3DB2Vj1XRiA4TZrts7el2eAt1KFOx6OkJnYVjNlJQK1+fLhZS04u4TJSg==" });

            migrationBuilder.InsertData(
                table: "DocumentTypes",
                columns: new[] { "Id", "IsCustomized", "Name" },
                values: new object[,]
                {
                    { 1, false, "Resume" },
                    { 2, false, "Job Decription" }
                });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 15, 16, 19, 32, 465, DateTimeKind.Local).AddTicks(5242));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentTypes");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 15, 10, 27, 0, 450, DateTimeKind.Local).AddTicks(9395), new DateTime(2024, 11, 15, 10, 27, 0, 450, DateTimeKind.Local).AddTicks(9396) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "4b190ff0-ce65-4fd6-b40d-fa7b16f8410e", new DateTime(2024, 11, 15, 10, 27, 0, 450, DateTimeKind.Local).AddTicks(9492), new DateTime(2024, 11, 15, 10, 27, 0, 450, DateTimeKind.Local).AddTicks(9493), "AQAAAAIAAYagAAAAEOqC7+3M7Nv5Ic36Mc+BoUBx+MQOpuckH//l9Xobea54Wl3uD6hYSyt7TCfgQrPl2g==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 15, 10, 27, 0, 450, DateTimeKind.Local).AddTicks(9382));
        }
    }
}
