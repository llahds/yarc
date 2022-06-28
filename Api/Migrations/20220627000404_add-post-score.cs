using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addpostscore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostScores",
                schema: "Social",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    Hot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Top = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    New = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Rising = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostScores", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_PostScores_Posts_PostId",
                        column: x => x.PostId,
                        principalSchema: "Social",
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 27, 0, 4, 4, 270, DateTimeKind.Utc).AddTicks(8237));

            migrationBuilder.CreateIndex(
                name: "IX_PostScores_Hot",
                schema: "Social",
                table: "PostScores",
                column: "Hot");

            migrationBuilder.CreateIndex(
                name: "IX_PostScores_New",
                schema: "Social",
                table: "PostScores",
                column: "New");

            migrationBuilder.CreateIndex(
                name: "IX_PostScores_Rising",
                schema: "Social",
                table: "PostScores",
                column: "Rising");

            migrationBuilder.CreateIndex(
                name: "IX_PostScores_Top",
                schema: "Social",
                table: "PostScores",
                column: "Top");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostScores",
                schema: "Social");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 24, 15, 40, 36, 739, DateTimeKind.Utc).AddTicks(3421));
        }
    }
}
