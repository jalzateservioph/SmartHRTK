using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateShiftAddNDFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NightDiffEnd",
                table: "Shift",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NightDiffStart",
                table: "Shift",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NightDiffEnd",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "NightDiffStart",
                table: "Shift");
        }
    }
}
