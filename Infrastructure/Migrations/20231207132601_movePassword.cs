using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class movePassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentGroups_Users_UserId",
                table: "StudentGroups");

            migrationBuilder.DropIndex(
                name: "IX_StudentGroups_UserId",
                table: "StudentGroups");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "StudentGroups");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "HashPassword" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 12, 7, 13, 26, 1, 106, DateTimeKind.Unspecified).AddTicks(397), new TimeSpan(0, 0, 0, 0, 0)), "$2a$11$W66DyVp2VVYec0b.giNhUu1yRD373L86vxhWHKfWPe2Npq.s7F9e." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "StudentGroups",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "HashPassword" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 12, 6, 9, 15, 7, 908, DateTimeKind.Unspecified).AddTicks(1104), new TimeSpan(0, 0, 0, 0, 0)), "$2b$10$mbDelsRAkbRwegOjerZ9sOl5L0aQ0Rl4jFO4ydUydLAjuoT3TRIWC" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentGroups_UserId",
                table: "StudentGroups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentGroups_Users_UserId",
                table: "StudentGroups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
