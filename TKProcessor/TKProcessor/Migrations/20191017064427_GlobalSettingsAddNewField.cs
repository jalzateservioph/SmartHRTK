using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class GlobalSettingsAddNewField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DPUserId",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CreateDTRForNoWorkDays",
                table: "GlobalSetting",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DPUserId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreateDTRForNoWorkDays",
                table: "GlobalSetting");
        }
    }
}
