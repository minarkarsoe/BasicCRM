using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Recsite_Ats.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ADD_Job_Location_And_Categories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobLocations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobSubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobSubCategories_JobCategories_JobCategoryId",
                        column: x => x.JobCategoryId,
                        principalTable: "JobCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobSubLocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobLocationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSubLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobSubLocations_JobLocations_JobLocationId",
                        column: x => x.JobLocationId,
                        principalTable: "JobLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 19, 12, 24, 44, 619, DateTimeKind.Local).AddTicks(5218), new DateTime(2024, 11, 19, 12, 24, 44, 619, DateTimeKind.Local).AddTicks(5219) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "30c6e0e7-35d1-4812-90cc-f4c6c35141e2", new DateTime(2024, 11, 19, 12, 24, 44, 619, DateTimeKind.Local).AddTicks(5333), new DateTime(2024, 11, 19, 12, 24, 44, 619, DateTimeKind.Local).AddTicks(5333), "AQAAAAIAAYagAAAAECgbY0C2+Rrm8xo4y5ITgvzwrZ+0RQI7bkUWHaGODM/TCXK//UZ62Ym8iLc8H9sLfA==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 19, 12, 24, 44, 619, DateTimeKind.Local).AddTicks(5204));

            migrationBuilder.CreateIndex(
                name: "IX_JobSubCategories_JobCategoryId",
                table: "JobSubCategories",
                column: "JobCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_JobSubLocations_JobLocationId",
                table: "JobSubLocations",
                column: "JobLocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobSubCategories");

            migrationBuilder.DropTable(
                name: "JobSubLocations");

            migrationBuilder.DropTable(
                name: "JobCategories");

            migrationBuilder.DropTable(
                name: "JobLocations");

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "EditedDate" },
                values: new object[] { new DateTime(2024, 11, 18, 14, 40, 31, 830, DateTimeKind.Local).AddTicks(9945), new DateTime(2024, 11, 18, 14, 40, 31, 830, DateTimeKind.Local).AddTicks(9945) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "EditedDate", "PasswordHash" },
                values: new object[] { "7ee10a3b-87f8-4aaf-a18f-8b3e6f50a64a", new DateTime(2024, 11, 18, 14, 40, 31, 831, DateTimeKind.Local).AddTicks(43), new DateTime(2024, 11, 18, 14, 40, 31, 831, DateTimeKind.Local).AddTicks(44), "AQAAAAIAAYagAAAAEMSULSVuTTW2a5LqZL/Yc/X+iN++0827Oo//6IoramccpoKnm6LvDcRMK3NyvD2nZw==" });

            migrationBuilder.UpdateData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1,
                column: "SeatRenewelDate",
                value: new DateTime(2024, 11, 18, 14, 40, 31, 830, DateTimeKind.Local).AddTicks(9933));
        }
    }
}
