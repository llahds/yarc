using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addpostvoteindexincludepostId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 27, 15, 29, 29, 40, DateTimeKind.Utc).AddTicks(3825));

            migrationBuilder.CreateIndex(
                name: "IX_PostVotes_Vote",
                schema: "Social",
                table: "PostVotes",
                column: "Vote")
                .Annotation("SqlServer:Include", new[] { "PostId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostVotes_Vote",
                schema: "Social",
                table: "PostVotes");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 27, 15, 27, 22, 372, DateTimeKind.Utc).AddTicks(2833));
        }
    }
}
