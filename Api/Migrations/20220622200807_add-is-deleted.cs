using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addisdeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Social",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Social",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Social",
                table: "Forums",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                schema: "Social",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 22, 20, 8, 7, 119, DateTimeKind.Utc).AddTicks(2196));

            migrationBuilder.CreateIndex(
                name: "IX_Users_IsDeleted",
                schema: "Social",
                table: "Users",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IsDeleted",
                schema: "Social",
                table: "Posts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_IsDeleted",
                schema: "Social",
                table: "Forums",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IsDeleted",
                schema: "Social",
                table: "Comments",
                column: "IsDeleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_IsDeleted",
                schema: "Social",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Posts_IsDeleted",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Forums_IsDeleted",
                schema: "Social",
                table: "Forums");

            migrationBuilder.DropIndex(
                name: "IX_Comments_IsDeleted",
                schema: "Social",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Social",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Social",
                table: "Forums");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                schema: "Social",
                table: "Comments");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 22, 15, 42, 59, 708, DateTimeKind.Utc).AddTicks(7584));
        }
    }
}
