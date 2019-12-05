using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class AddWorkSiteFieldsToWorkSched : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkSiteId",
                table: "WorkSchedule",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedule_WorkSiteId",
                table: "WorkSchedule",
                column: "WorkSiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedule_WorkSite_WorkSiteId",
                table: "WorkSchedule",
                column: "WorkSiteId",
                principalTable: "WorkSite",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedule_WorkSite_WorkSiteId",
                table: "WorkSchedule");

            migrationBuilder.DropIndex(
                name: "IX_WorkSchedule_WorkSiteId",
                table: "WorkSchedule");

            migrationBuilder.DropColumn(
                name: "WorkSiteId",
                table: "WorkSchedule");
        }
    }
}
