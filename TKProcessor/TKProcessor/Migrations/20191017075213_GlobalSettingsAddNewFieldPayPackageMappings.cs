using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class GlobalSettingsAddNewFieldPayPackageMappings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GlobalSettingId1",
                table: "Mapping",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mapping_GlobalSettingId1",
                table: "Mapping",
                column: "GlobalSettingId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Mapping_GlobalSetting_GlobalSettingId1",
                table: "Mapping",
                column: "GlobalSettingId1",
                principalTable: "GlobalSetting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mapping_GlobalSetting_GlobalSettingId1",
                table: "Mapping");

            migrationBuilder.DropIndex(
                name: "IX_Mapping_GlobalSettingId1",
                table: "Mapping");

            migrationBuilder.DropColumn(
                name: "GlobalSettingId1",
                table: "Mapping");
        }
    }
}
