using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateRawDataRemovedScheduleDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RawData_User_CreatedById",
                table: "RawData");

            migrationBuilder.DropForeignKey(
                name: "FK_RawData_User_LastModifiedById",
                table: "RawData");

            migrationBuilder.DropIndex(
                name: "IX_RawData_CreatedById",
                table: "RawData");

            migrationBuilder.DropIndex(
                name: "IX_RawData_LastModifiedById",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                table: "WorkSchedule");

            migrationBuilder.DropColumn(
                name: "ShiftCode",
                table: "WorkSchedule");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "ScheduleDate",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "ShiftCode",
                table: "Employee");

            migrationBuilder.RenameColumn(
                name: "HolidayType",
                table: "Holiday",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "HolidayName",
                table: "Holiday",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "HolidayDate",
                table: "Holiday",
                newName: "Date");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "WorkSchedule",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShiftCodeId",
                table: "WorkSchedule",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedule_EmployeeId",
                table: "WorkSchedule",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedule_ShiftCodeId",
                table: "WorkSchedule",
                column: "ShiftCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedule_Employee_EmployeeId",
                table: "WorkSchedule",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedule_Shift_ShiftCodeId",
                table: "WorkSchedule",
                column: "ShiftCodeId",
                principalTable: "Shift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedule_Employee_EmployeeId",
                table: "WorkSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedule_Shift_ShiftCodeId",
                table: "WorkSchedule");

            migrationBuilder.DropIndex(
                name: "IX_WorkSchedule_EmployeeId",
                table: "WorkSchedule");

            migrationBuilder.DropIndex(
                name: "IX_WorkSchedule_ShiftCodeId",
                table: "WorkSchedule");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "WorkSchedule");

            migrationBuilder.DropColumn(
                name: "ShiftCodeId",
                table: "WorkSchedule");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Holiday",
                newName: "HolidayType");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Holiday",
                newName: "HolidayName");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Holiday",
                newName: "HolidayDate");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCode",
                table: "WorkSchedule",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShiftCode",
                table: "WorkSchedule",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "RawData",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "RawData",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "RawData",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "RawData",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleDate",
                table: "RawData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShiftCode",
                table: "Employee",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RawData_CreatedById",
                table: "RawData",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RawData_LastModifiedById",
                table: "RawData",
                column: "LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_RawData_User_CreatedById",
                table: "RawData",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RawData_User_LastModifiedById",
                table: "RawData",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
