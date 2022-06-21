using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addforummembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ForumMembers",
                schema: "Social",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ForumId = table.Column<int>(type: "int", nullable: false),
                    MemberId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForumMembers_Forums_ForumId",
                        column: x => x.ForumId,
                        principalSchema: "Social",
                        principalTable: "Forums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumMembers_Users_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "Social",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 21, 15, 32, 12, 70, DateTimeKind.Utc).AddTicks(4135));

            migrationBuilder.CreateIndex(
                name: "IX_ForumMembers_ForumId",
                schema: "Social",
                table: "ForumMembers",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumMembers_MemberId",
                schema: "Social",
                table: "ForumMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumMembers_Status",
                schema: "Social",
                table: "ForumMembers",
                column: "Status");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumMembers",
                schema: "Social");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 21, 13, 48, 18, 200, DateTimeKind.Utc).AddTicks(5536));
        }
    }
}
