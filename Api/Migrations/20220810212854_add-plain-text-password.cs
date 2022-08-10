using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addplaintextpassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PlainTextPassword",
                schema: "Social",
                table: "Users",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Forums",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 8, 10, 21, 28, 53, 678, DateTimeKind.Utc).AddTicks(8397));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 8, 10, 21, 28, 53, 678, DateTimeKind.Utc).AddTicks(8407));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "CreatedOn", "PlainTextPassword" },
                values: new object[] { new DateTime(2022, 8, 10, 21, 28, 53, 678, DateTimeKind.Utc).AddTicks(8284), "" });

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "PlainTextPassword" },
                values: new object[] { new DateTime(2022, 8, 10, 21, 28, 53, 678, DateTimeKind.Utc).AddTicks(8266), "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlainTextPassword",
                schema: "Social",
                table: "Users");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Forums",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 20, 19, 29, 5, 379, DateTimeKind.Utc).AddTicks(5397));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 20, 19, 29, 5, 379, DateTimeKind.Utc).AddTicks(5408));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 20, 19, 29, 5, 379, DateTimeKind.Utc).AddTicks(5296));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 20, 19, 29, 5, 379, DateTimeKind.Utc).AddTicks(5279));
        }
    }
}
