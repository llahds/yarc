using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addisspamtoposts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Social",
                table: "ReportReasons",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsSpam",
                schema: "Social",
                table: "Posts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: -1,
                column: "Code",
                value: "TOXICITY");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: "FORUM_RULES");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: "HARASSMENT");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: "VIOLENCE");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: "HATE");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: "SEXUALIZATION");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: "PII");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 7,
                column: "Code",
                value: "NCIM");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 8,
                column: "Code",
                value: "PT");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 9,
                column: "Code",
                value: "IMPERSONATION");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 10,
                column: "Code",
                value: "COPYRIGHT");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 11,
                column: "Code",
                value: "TRADEMARK");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 12,
                column: "Code",
                value: "SELF_HARM");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 13,
                column: "Code",
                value: "SPAM");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: 14,
                column: "Code",
                value: "MISINFORMATION");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 15, 19, 0, 6, 839, DateTimeKind.Utc).AddTicks(4470));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 15, 19, 0, 6, 839, DateTimeKind.Utc).AddTicks(4455));

            migrationBuilder.CreateIndex(
                name: "IX_ReportReasons_Code",
                schema: "Social",
                table: "ReportReasons",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ReportReasons_Code",
                schema: "Social",
                table: "ReportReasons");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Social",
                table: "ReportReasons");

            migrationBuilder.DropColumn(
                name: "IsSpam",
                schema: "Social",
                table: "Posts");

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: -1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 8, 14, 39, 20, 849, DateTimeKind.Utc).AddTicks(7274));

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 8, 14, 39, 20, 849, DateTimeKind.Utc).AddTicks(7257));
        }
    }
}
