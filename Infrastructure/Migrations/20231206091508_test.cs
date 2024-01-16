using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "HashPassword" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 12, 6, 9, 15, 7, 908, DateTimeKind.Unspecified).AddTicks(1104), new TimeSpan(0, 0, 0, 0, 0)), "$2b$10$mbDelsRAkbRwegOjerZ9sOl5L0aQ0Rl4jFO4ydUydLAjuoT3TRIWC" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "HashPassword" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 12, 6, 8, 34, 18, 118, DateTimeKind.Unspecified).AddTicks(4645), new TimeSpan(0, 0, 0, 0, 0)), "$2b$10$WRVwALXmofdfejEeGr9kEe0D035Ej4FaUkQfwclEoh5XrO5pp0H0O" });
        }
    }
}
