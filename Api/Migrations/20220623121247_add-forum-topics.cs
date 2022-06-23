using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addforumtopics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Topics",
                schema: "Social",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForumTopic",
                schema: "Social",
                columns: table => new
                {
                    ForumId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumTopic", x => new { x.ForumId, x.TopicId });
                    table.ForeignKey(
                        name: "FK_ForumTopic_Forums_ForumId",
                        column: x => x.ForumId,
                        principalSchema: "Social",
                        principalTable: "Forums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumTopic_Topics_TopicId",
                        column: x => x.TopicId,
                        principalSchema: "Social",
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 23, 12, 12, 46, 915, DateTimeKind.Utc).AddTicks(3603));

            migrationBuilder.CreateIndex(
                name: "IX_ForumTopic_TopicId",
                schema: "Social",
                table: "ForumTopic",
                column: "TopicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumTopic",
                schema: "Social");

            migrationBuilder.DropTable(
                name: "Topics",
                schema: "Social");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 22, 20, 29, 27, 727, DateTimeKind.Utc).AddTicks(8638));
        }
    }
}
