using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateShiftEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EarliestTimeIn",
                table: "Shift",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EarliestTimeOut",
                table: "Shift",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FlextimeType",
                table: "Shift",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Increment",
                table: "Shift",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOverbreak",
                table: "Shift",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LatestTimeIn",
                table: "Shift",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LatestTimeOut",
                table: "Shift",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EarliestTimeIn",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "EarliestTimeOut",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "FlextimeType",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "Increment",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "IsOverbreak",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "LatestTimeIn",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "LatestTimeOut",
                table: "Shift");
        }
    }
}
