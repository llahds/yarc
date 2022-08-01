using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addusersettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                schema: "Social",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSettings_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "Social",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Forums",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 8, 1, 19, 38, 24, 595, DateTimeKind.Utc).AddTicks(8745));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Posts",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 8, 1, 19, 38, 24, 595, DateTimeKind.Utc).AddTicks(8757));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedOn",
                value: new DateTime(2022, 8, 1, 19, 38, 24, 595, DateTimeKind.Utc).AddTicks(8617));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 8, 1, 19, 38, 24, 595, DateTimeKind.Utc).AddTicks(8594));

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_Key",
                schema: "Social",
                table: "UserSettings",
                column: "Key");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                schema: "Social",
                table: "UserSettings",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings",
                schema: "Social");

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
