using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class changepostisdeletedishiddenindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_IsDeleted",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_IsHidden",
                schema: "Social",
                table: "Posts");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 8, 13, 1, 22, 886, DateTimeKind.Utc).AddTicks(967));

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IsHidden_IsDeleted",
                schema: "Social",
                table: "Posts",
                columns: new[] { "IsHidden", "IsDeleted" })
                .Annotation("SqlServer:Include", new[] { "ForumId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Posts_IsHidden_IsDeleted",
                schema: "Social",
                table: "Posts");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 6, 12, 24, 22, 404, DateTimeKind.Utc).AddTicks(4796));

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IsDeleted",
                schema: "Social",
                table: "Posts",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IsHidden",
                schema: "Social",
                table: "Posts",
                column: "IsHidden");
        }
    }
}
