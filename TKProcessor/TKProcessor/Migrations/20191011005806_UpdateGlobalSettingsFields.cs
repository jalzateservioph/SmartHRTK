using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateGlobalSettingsFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "GlobalSetting");

            migrationBuilder.AddColumn<DateTime>(
                name: "DefaultNDEnd",
                table: "GlobalSetting",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DefaultNDStart",
                table: "GlobalSetting",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultNDEnd",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "DefaultNDStart",
                table: "GlobalSetting");

            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "GlobalSetting",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "GlobalSetting",
                nullable: true);
        }
    }
}
