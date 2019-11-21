using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class GlobalSettingsAddedAndRemovedFields1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SelectionSetting",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "SelectionSetting",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
