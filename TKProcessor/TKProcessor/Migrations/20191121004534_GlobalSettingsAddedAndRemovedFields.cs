using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class GlobalSettingsAddedAndRemovedFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoApproveDTRFields",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "CreateDTRForNoWorkDays",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "DisplayDTRColumnsAsMinutes",
                table: "GlobalSetting");

            migrationBuilder.CreateTable(
                name: "SelectionSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    Name = table.Column<int>(nullable: false),
                    IsSelected = table.Column<bool>(nullable: false),
                    GlobalSettingId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelectionSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelectionSetting_GlobalSetting_GlobalSettingId",
                        column: x => x.GlobalSettingId,
                        principalTable: "GlobalSetting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SelectionSetting_GlobalSettingId",
                table: "SelectionSetting",
                column: "GlobalSettingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelectionSetting");

            migrationBuilder.AddColumn<bool>(
                name: "AutoApproveDTRFields",
                table: "GlobalSetting",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CreateDTRForNoWorkDays",
                table: "GlobalSetting",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "DisplayDTRColumnsAsMinutes",
                table: "GlobalSetting",
                nullable: false,
                defaultValue: false);
        }
    }
}
