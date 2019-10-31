using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class AddedMappingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mapping",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Target = table.Column<string>(nullable: true),
                    Source = table.Column<string>(nullable: true),
                    GlobalSettingId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mapping", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mapping_GlobalSetting_GlobalSettingId",
                        column: x => x.GlobalSettingId,
                        principalTable: "GlobalSetting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Mapping_GlobalSettingId",
                table: "Mapping",
                column: "GlobalSettingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mapping");
        }
    }
}
