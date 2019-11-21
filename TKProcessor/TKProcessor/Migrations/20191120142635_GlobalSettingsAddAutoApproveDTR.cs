using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class GlobalSettingsAddAutoApproveDTR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoApproveDTRFields",
                table: "GlobalSetting",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoApproveDTRFields",
                table: "GlobalSetting");
        }
    }
}
