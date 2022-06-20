using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class adduniqueuserdisplayname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DisplayName" },
                values: new object[] { new DateTime(2022, 6, 20, 13, 41, 30, 737, DateTimeKind.Utc).AddTicks(5628), "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DisplayName",
                schema: "Social",
                table: "Users",
                column: "DisplayName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_DisplayName",
                schema: "Social",
                table: "Users");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DisplayName" },
                values: new object[] { new DateTime(2022, 6, 18, 20, 9, 16, 577, DateTimeKind.Utc).AddTicks(3209), "" });
        }
    }
}
