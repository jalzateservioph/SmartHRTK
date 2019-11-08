using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateGlobalSettingsAddFieldDTRColumnsToMinutes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DisplayDTRColumnsAsMinutes",
                table: "GlobalSetting",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayDTRColumnsAsMinutes",
                table: "GlobalSetting");
        }
    }
}
