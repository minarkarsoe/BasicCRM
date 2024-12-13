using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_CompanyFollowers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyFollowers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyFollowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyFollowers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyFollowers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_CompanyFollowers_CompanyId",
                table: "CompanyFollowers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyFollowers_UserId",
                table: "CompanyFollowers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyFollowers");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 7, 15, 49, 5, 880, DateTimeKind.Local).AddTicks(3654), new DateTime(2024, 11, 7, 15, 49, 5, 880, DateTimeKind.Local).AddTicks(3655) });

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
    }
}
