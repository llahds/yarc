using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addusername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_DisplayName",
                schema: "Social",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                schema: "Social",
                table: "Users",
                type: "nvarchar(35)",
                maxLength: 35,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "DisplayName", "UserName" },
                values: new object[] { new DateTime(2022, 6, 20, 14, 9, 39, 189, DateTimeKind.Utc).AddTicks(3709), "Administrator", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "Social",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                schema: "Social",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserName",
                schema: "Social",
                table: "Users");

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
    }
}
