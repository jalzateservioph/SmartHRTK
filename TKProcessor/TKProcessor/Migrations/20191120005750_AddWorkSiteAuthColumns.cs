using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class AddWorkSiteAuthColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IntegrationAuthPassword",
                table: "WorkSite",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IntegrationAuthUsername",
                table: "WorkSite",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntegrationAuthPassword",
                table: "WorkSite");

            migrationBuilder.DropColumn(
                name: "IntegrationAuthUsername",
                table: "WorkSite");
        }
    }
}
