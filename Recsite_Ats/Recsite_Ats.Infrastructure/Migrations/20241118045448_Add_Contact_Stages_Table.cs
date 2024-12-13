using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Contact_Stages_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactStages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCustomized = table.Column<bool>(type: "bit", nullable: false),
                    Sort = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactStages", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "ContactStages",
                columns: new[] { "Id", "IsCustomized", "Name", "Sort" },
                values: new object[,]
                {
                    { 1, false, "Phone Call", 1 },
                    { 2, false, "Email", 2 }
                });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 18, 11, 54, 47, 348, DateTimeKind.Local).AddTicks(232));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactStages");

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

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 15, 16, 19, 32, 465, DateTimeKind.Local).AddTicks(5242));
        }
    }
}
