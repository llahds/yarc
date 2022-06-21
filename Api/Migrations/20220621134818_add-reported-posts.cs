using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addreportedposts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                schema: "Social",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ReportReasons",
                schema: "Social",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Label = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportedPosts",
                schema: "Social",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportedById = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    ReasonId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportedPosts_Posts_PostId",
                        column: x => x.PostId,
                        principalSchema: "Social",
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedPosts_ReportReasons_ReasonId",
                        column: x => x.ReasonId,
                        principalSchema: "Social",
                        principalTable: "ReportReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedPosts_Users_ReportedById",
                        column: x => x.ReportedById,
                        principalSchema: "Social",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "Social",
                table: "ReportReasons",
                columns: new[] { "Id", "Label" },
                values: new object[,]
                {
                    { 1, "Breaks {forum} rules" },
                    { 2, "Harassment" },
                    { 3, "Threatening violence" },
                    { 4, "Hate" },
                    { 5, "Sexualization of minors" },
                    { 6, "Sharing personal information" },
                    { 7, "Non-consentual intimate media" },
                    { 8, "Prohibited transaction" },
                    { 9, "Impersonation" },
                    { 10, "Copyright violation" },
                    { 11, "Trademark violation" },
                    { 12, "Self-harm or suicide" },
                    { 13, "Spam" },
                    { 14, "Misinformation" }
                });

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 21, 13, 48, 18, 200, DateTimeKind.Utc).AddTicks(5536));

            migrationBuilder.CreateIndex(
                name: "IX_Posts_IsHidden",
                schema: "Social",
                table: "Posts",
                column: "IsHidden");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedPosts_PostId",
                schema: "Social",
                table: "ReportedPosts",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedPosts_ReasonId",
                schema: "Social",
                table: "ReportedPosts",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedPosts_ReportedById",
                schema: "Social",
                table: "ReportedPosts",
                column: "ReportedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportedPosts",
                schema: "Social");

            migrationBuilder.DropTable(
                name: "ReportReasons",
                schema: "Social");

            migrationBuilder.DropIndex(
                name: "IX_Posts_IsHidden",
                schema: "Social",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                schema: "Social",
                table: "Posts");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 20, 14, 9, 39, 189, DateTimeKind.Utc).AddTicks(3709));
        }
    }
}
