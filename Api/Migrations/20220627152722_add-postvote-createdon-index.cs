using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addpostvotecreatedonindex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 27, 15, 27, 22, 372, DateTimeKind.Utc).AddTicks(2833));

            migrationBuilder.CreateIndex(
                name: "IX_PostVotes_CreatedOn",
                schema: "Social",
                table: "PostVotes",
                column: "CreatedOn")
                .Annotation("SqlServer:Include", new[] { "PostId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostVotes_CreatedOn",
                schema: "Social",
                table: "PostVotes");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 27, 0, 4, 4, 270, DateTimeKind.Utc).AddTicks(8237));
        }
    }
}
