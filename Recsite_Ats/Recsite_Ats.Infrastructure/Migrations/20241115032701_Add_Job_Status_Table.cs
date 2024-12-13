using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Job_Status_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sort = table.Column<int>(type: "int", nullable: false),
                    IsCustomized = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobStatus", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "JobStatus",
                columns: new[] { "Id", "IsCustomized", "Name", "Sort" },
                values: new object[,]
                {
                    { 1, false, "Open", 1 },
                    { 2, false, "On Hold", 2 },
                    { 3, false, "Canceled", 3 },
                    { 4, false, "Closed", 4 }
                });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 15, 10, 27, 0, 450, DateTimeKind.Local).AddTicks(9382));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobStatus");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 14, 11, 0, 11, 80, DateTimeKind.Local).AddTicks(4211), new DateTime(2024, 11, 14, 11, 0, 11, 80, DateTimeKind.Local).AddTicks(4211) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "db758625-5407-4673-b297-77ce758bdfa5", new DateTime(2024, 11, 14, 11, 0, 11, 80, DateTimeKind.Local).AddTicks(4341), new DateTime(2024, 11, 14, 11, 0, 11, 80, DateTimeKind.Local).AddTicks(4341), "AQAAAAIAAYagAAAAEDTC0mJNlmlVZCxZ4Q3E1WFhM6ku9VHrRXw/3csjYiB1EFfv8kOQZdro31pZSJ61aw==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 14, 11, 0, 11, 80, DateTimeKind.Local).AddTicks(4196));
        }
    }
}
