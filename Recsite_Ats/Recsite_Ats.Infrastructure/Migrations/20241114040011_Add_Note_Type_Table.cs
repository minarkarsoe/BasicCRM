using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Note_Type_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NoteTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsCustomize = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTypes", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "NoteTypes",
                columns: new[] { "Id", "IsCustomize", "IsDefault", "Name" },
                values: new object[,]
                {
                    { 1, false, false, "Call" },
                    { 2, false, false, "To Do" }
                });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 14, 11, 0, 11, 80, DateTimeKind.Local).AddTicks(4196));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteTypes");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 12, 16, 54, 26, 302, DateTimeKind.Local).AddTicks(785), new DateTime(2024, 11, 12, 16, 54, 26, 302, DateTimeKind.Local).AddTicks(786) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "e4d2de92-bf28-4bf1-9ff4-2433b0ee37fa", new DateTime(2024, 11, 12, 16, 54, 26, 302, DateTimeKind.Local).AddTicks(888), new DateTime(2024, 11, 12, 16, 54, 26, 302, DateTimeKind.Local).AddTicks(889), "AQAAAAIAAYagAAAAEBxyns81hrLo1IkArnCeh4DmRSxDxTBKf1ETbx3BPuvaejB9QVA0aF+aqbhq1wO8Fw==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 12, 16, 54, 26, 302, DateTimeKind.Local).AddTicks(773));
        }
    }
}
