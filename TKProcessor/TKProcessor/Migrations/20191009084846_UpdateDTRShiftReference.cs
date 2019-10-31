using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateDTRShiftReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShiftCode",
                table: "DailyTransactionRecord");

            migrationBuilder.AddColumn<Guid>(
                name: "ShiftId",
                table: "DailyTransactionRecord",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyTransactionRecord_ShiftId",
                table: "DailyTransactionRecord",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTransactionRecord_Shift_ShiftId",
                table: "DailyTransactionRecord",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyTransactionRecord_Shift_ShiftId",
                table: "DailyTransactionRecord");

            migrationBuilder.DropIndex(
                name: "IX_DailyTransactionRecord_ShiftId",
                table: "DailyTransactionRecord");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "DailyTransactionRecord");

            migrationBuilder.AddColumn<string>(
                name: "ShiftCode",
                table: "DailyTransactionRecord",
                nullable: true);
        }
    }
}
