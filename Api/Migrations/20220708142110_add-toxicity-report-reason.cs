using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class addtoxicityreportreason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Social",
                table: "ReportReasons",
                columns: new[] { "Id", "Label" },
                values: new object[] { -1, "Toxicity" });

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 8, 14, 21, 10, 632, DateTimeKind.Utc).AddTicks(9807));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Social",
                table: "ReportReasons",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.UpdateData(
                schema: "Social",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2022, 7, 8, 13, 1, 22, 886, DateTimeKind.Utc).AddTicks(967));
        }
    }
}
