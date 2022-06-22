using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addreportedcomments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                schema: "Social",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ReportedComments",
                schema: "Social",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportedById = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    ReasonId = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportedComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportedComments_Comments_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "Social",
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedComments_ReportReasons_ReasonId",
                        column: x => x.ReasonId,
                        principalSchema: "Social",
                        principalTable: "ReportReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportedComments_Users_ReportedById",
                        column: x => x.ReportedById,
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
                value: new DateTime(2022, 6, 22, 20, 29, 27, 727, DateTimeKind.Utc).AddTicks(8638));

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IsHidden",
                schema: "Social",
                table: "Comments",
                column: "IsHidden");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedComments_CommentId",
                schema: "Social",
                table: "ReportedComments",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedComments_ReasonId",
                schema: "Social",
                table: "ReportedComments",
                column: "ReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportedComments_ReportedById",
                schema: "Social",
                table: "ReportedComments",
                column: "ReportedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportedComments",
                schema: "Social");

            migrationBuilder.DropIndex(
                name: "IX_Comments_IsHidden",
                schema: "Social",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsHidden",
                schema: "Social",
                table: "Comments");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 6, 22, 20, 8, 7, 119, DateTimeKind.Utc).AddTicks(2196));
        }
    }
}
