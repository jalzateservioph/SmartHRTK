using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateModelsWorkScheduleShiftField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedule_Shift_ShiftCodeId",
                table: "WorkSchedule");

            migrationBuilder.RenameColumn(
                name: "ShiftCodeId",
                table: "WorkSchedule",
                newName: "ShiftId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkSchedule_ShiftCodeId",
                table: "WorkSchedule",
                newName: "IX_WorkSchedule_ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedule_Shift_ShiftId",
                table: "WorkSchedule",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedule_Shift_ShiftId",
                table: "WorkSchedule");

            migrationBuilder.RenameColumn(
                name: "ShiftId",
                table: "WorkSchedule",
                newName: "ShiftCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkSchedule_ShiftId",
                table: "WorkSchedule",
                newName: "IX_WorkSchedule_ShiftCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedule_Shift_ShiftCodeId",
                table: "WorkSchedule",
                column: "ShiftCodeId",
                principalTable: "Shift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
