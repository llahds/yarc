using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addyarcbotuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 8, 14, 39, 20, 849, DateTimeKind.Utc).AddTicks(7257));

            migrationBuilder.InsertData(
                schema: "Social",
                table: "Users",
                columns: new[] { "Id", "About", "CreatedOn", "DisplayName", "Email", "IsDeleted", "Password", "UserName" },
                values: new object[] { -1, "", new DateTime(2022, 7, 8, 14, 39, 20, 849, DateTimeKind.Utc).AddTicks(7274), "YARCBot", "YARCBot", true, "_________", "YARCBot" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 8, 14, 21, 10, 632, DateTimeKind.Utc).AddTicks(9807));
        }
    }
}
