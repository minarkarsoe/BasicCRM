using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Candidate_Source_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CandidateSources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCustomized = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateSources", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 18, 14, 25, 31, 284, DateTimeKind.Local).AddTicks(2628), new DateTime(2024, 11, 18, 14, 25, 31, 284, DateTimeKind.Local).AddTicks(2629) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "478227e5-aed1-43b4-b568-b8f3e618b152", new DateTime(2024, 11, 18, 14, 25, 31, 284, DateTimeKind.Local).AddTicks(2773), new DateTime(2024, 11, 18, 14, 25, 31, 284, DateTimeKind.Local).AddTicks(2774), "AQAAAAIAAYagAAAAEJaIe/zFFIGXC986rpG7DC1GPaIAnXNLmhvhtrHJWY3wj0XZgm6237bjPnzCAStsTg==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 18, 14, 25, 31, 284, DateTimeKind.Local).AddTicks(2612));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateSources");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 18, 11, 54, 47, 348, DateTimeKind.Local).AddTicks(247), new DateTime(2024, 11, 18, 11, 54, 47, 348, DateTimeKind.Local).AddTicks(247) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "bad54a11-a3c3-4f79-9755-3ed34b10d896", new DateTime(2024, 11, 18, 11, 54, 47, 348, DateTimeKind.Local).AddTicks(354), new DateTime(2024, 11, 18, 11, 54, 47, 348, DateTimeKind.Local).AddTicks(355), "AQAAAAIAAYagAAAAEI2+iipb692X0gkamkcOFDhTJzZqcsq1bE3IBBVF9BlaJBRSs9m2mulPZCFhrSPdnA==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 18, 11, 54, 47, 348, DateTimeKind.Local).AddTicks(232));
        }
    }
}
