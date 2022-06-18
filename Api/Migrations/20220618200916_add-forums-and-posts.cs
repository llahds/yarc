using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addforumsandposts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Social");

            migrationBuilder.CreateTable(
                name: "Forums",
                schema: "Social",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forums", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "Social",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    About = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForumModerators",
                schema: "Social",
                columns: table => new
                {
                    ModeratorId = table.Column<int>(type: "int", nullable: false),
                    ForumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumModerators", x => new { x.ForumId, x.ModeratorId });
                    table.ForeignKey(
                        name: "FK_ForumModerators_Forums_ForumId",
                        column: x => x.ForumId,
                        principalSchema: "Social",
                        principalTable: "Forums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumModerators_Users_ModeratorId",
                        column: x => x.ModeratorId,
                        principalSchema: "Social",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ForumOwners",
                schema: "Social",
                columns: table => new
                {
                    OwnerId = table.Column<int>(type: "int", nullable: false),
                    ForumId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForumOwners", x => new { x.ForumId, x.OwnerId });
                    table.ForeignKey(
                        name: "FK_ForumOwners_Forums_ForumId",
                        column: x => x.ForumId,
                        principalSchema: "Social",
                        principalTable: "Forums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForumOwners_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "Social",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                schema: "Social",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ForumId = table.Column<int>(type: "int", nullable: false),
                    PostedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Forums_ForumId",
                        column: x => x.ForumId,
                        principalSchema: "Social",
                        principalTable: "Forums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Users_PostedById",
                        column: x => x.PostedById,
                        principalSchema: "Social",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Social",
                table: "Users",
                columns: new[] { "Id", "About", "CreatedOn", "DisplayName", "Email", "Password" },
                values: new object[] { 1, "", new DateTime(2022, 6, 18, 20, 9, 16, 577, DateTimeKind.Utc).AddTicks(3209), "", "admin", "password" });

            migrationBuilder.CreateIndex(
                name: "IX_ForumModerators_ModeratorId",
                schema: "Social",
                table: "ForumModerators",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "IX_ForumOwners_OwnerId",
                schema: "Social",
                table: "ForumOwners",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Forums_Slug",
                schema: "Social",
                table: "Forums",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CreatedOn",
                schema: "Social",
                table: "Posts",
                column: "CreatedOn");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ForumId",
                schema: "Social",
                table: "Posts",
                column: "ForumId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostedById",
                schema: "Social",
                table: "Posts",
                column: "PostedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "Social",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForumModerators",
                schema: "Social");

            migrationBuilder.DropTable(
                name: "ForumOwners",
                schema: "Social");

            migrationBuilder.DropTable(
                name: "Posts",
                schema: "Social");

            migrationBuilder.DropTable(
                name: "Forums",
                schema: "Social");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "Social");
        }
    }
}
