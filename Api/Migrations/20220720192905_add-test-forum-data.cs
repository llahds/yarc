using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addtestforumdata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Social",
                table: "Forums",
                columns: new[] { "Id", "CreatedOn", "Description", "IsDeleted", "IsPrivate", "Name", "Slug", "PostSettings" },
                values: new object[] { 1, new DateTime(2022, 7, 20, 19, 29, 5, 379, DateTimeKind.Utc).AddTicks(5397), "", false, false, "First Forum", "first-forum", "" });

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

            migrationBuilder.InsertData(
                schema: "Social",
                table: "Posts",
                columns: new[] { "Id", "CommentCount", "CreatedOn", "Downs", "ForumId", "Hot", "IsDeleted", "IsHidden", "IsSpam", "New", "PostedById", "Rising", "Text", "Title", "Top", "Ups" },
                values: new object[] { 1, 0, new DateTime(2022, 7, 20, 19, 29, 5, 379, DateTimeKind.Utc).AddTicks(5408), 0, 1, 0m, false, false, false, 0m, 1, 0m, "Test Comments", "Test", 0m, 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Social",
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Social",
                table: "Forums",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 15, 19, 0, 6, 839, DateTimeKind.Utc).AddTicks(4470));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 15, 19, 0, 6, 839, DateTimeKind.Utc).AddTicks(4455));
        }
    }
}
