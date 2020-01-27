using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class AuditLogRevamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLog_User_CreatedById",
                table: "AuditLog");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditLog_User_LastModifiedById",
                table: "AuditLog");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarDays_User_CreatedById",
                table: "CalendarDays");

            migrationBuilder.DropForeignKey(
                name: "FK_CalendarDays_User_LastModifiedById",
                table: "CalendarDays");

            migrationBuilder.DropForeignKey(
                name: "FK_Calendars_User_CreatedById",
                table: "Calendars");

            migrationBuilder.DropForeignKey(
                name: "FK_Calendars_User_LastModifiedById",
                table: "Calendars");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyTransactionRecord_User_CreatedById",
                table: "DailyTransactionRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_DailyTransactionRecord_User_LastModifiedById",
                table: "DailyTransactionRecord");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_User_CreatedById",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_Employee_User_LastModifiedById",
                table: "Employee");

            migrationBuilder.DropForeignKey(
                name: "FK_ErrorLog_User_CreatedById",
                table: "ErrorLog");

            migrationBuilder.DropForeignKey(
                name: "FK_ErrorLog_User_LastModifiedById",
                table: "ErrorLog");

            migrationBuilder.DropForeignKey(
                name: "FK_GlobalSetting_User_CreatedById",
                table: "GlobalSetting");

            migrationBuilder.DropForeignKey(
                name: "FK_GlobalSetting_User_LastModifiedById",
                table: "GlobalSetting");

            migrationBuilder.DropForeignKey(
                name: "FK_Holiday_User_CreatedById",
                table: "Holiday");

            migrationBuilder.DropForeignKey(
                name: "FK_Holiday_User_LastModifiedById",
                table: "Holiday");

            migrationBuilder.DropForeignKey(
                name: "FK_Leave_User_CreatedById",
                table: "Leave");

            migrationBuilder.DropForeignKey(
                name: "FK_Leave_User_LastModifiedById",
                table: "Leave");

            migrationBuilder.DropForeignKey(
                name: "FK_RawData_User_CreatedById",
                table: "RawData");

            migrationBuilder.DropForeignKey(
                name: "FK_RawData_User_LastModifiedById",
                table: "RawData");

            migrationBuilder.DropForeignKey(
                name: "FK_Shift_User_CreatedById",
                table: "Shift");

            migrationBuilder.DropForeignKey(
                name: "FK_Shift_User_LastModifiedById",
                table: "Shift");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedule_User_CreatedById",
                table: "WorkSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedule_User_LastModifiedById",
                table: "WorkSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSite_User_CreatedById",
                table: "WorkSite");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSite_User_LastModifiedById",
                table: "WorkSite");

            migrationBuilder.DropIndex(
                name: "IX_WorkSite_CreatedById",
                table: "WorkSite");

            migrationBuilder.DropIndex(
                name: "IX_WorkSite_LastModifiedById",
                table: "WorkSite");

            migrationBuilder.DropIndex(
                name: "IX_WorkSchedule_CreatedById",
                table: "WorkSchedule");

            migrationBuilder.DropIndex(
                name: "IX_WorkSchedule_LastModifiedById",
                table: "WorkSchedule");

            migrationBuilder.DropIndex(
                name: "IX_Shift_CreatedById",
                table: "Shift");

            migrationBuilder.DropIndex(
                name: "IX_Shift_LastModifiedById",
                table: "Shift");

            migrationBuilder.DropIndex(
                name: "IX_RawData_CreatedById",
                table: "RawData");

            migrationBuilder.DropIndex(
                name: "IX_RawData_LastModifiedById",
                table: "RawData");

            migrationBuilder.DropIndex(
                name: "IX_Leave_CreatedById",
                table: "Leave");

            migrationBuilder.DropIndex(
                name: "IX_Leave_LastModifiedById",
                table: "Leave");

            migrationBuilder.DropIndex(
                name: "IX_Holiday_CreatedById",
                table: "Holiday");

            migrationBuilder.DropIndex(
                name: "IX_Holiday_LastModifiedById",
                table: "Holiday");

            migrationBuilder.DropIndex(
                name: "IX_GlobalSetting_CreatedById",
                table: "GlobalSetting");

            migrationBuilder.DropIndex(
                name: "IX_GlobalSetting_LastModifiedById",
                table: "GlobalSetting");

            migrationBuilder.DropIndex(
                name: "IX_ErrorLog_CreatedById",
                table: "ErrorLog");

            migrationBuilder.DropIndex(
                name: "IX_ErrorLog_LastModifiedById",
                table: "ErrorLog");

            migrationBuilder.DropIndex(
                name: "IX_Employee_CreatedById",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_LastModifiedById",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_DailyTransactionRecord_CreatedById",
                table: "DailyTransactionRecord");

            migrationBuilder.DropIndex(
                name: "IX_DailyTransactionRecord_LastModifiedById",
                table: "DailyTransactionRecord");

            migrationBuilder.DropIndex(
                name: "IX_Calendars_CreatedById",
                table: "Calendars");

            migrationBuilder.DropIndex(
                name: "IX_Calendars_LastModifiedById",
                table: "Calendars");

            migrationBuilder.DropIndex(
                name: "IX_CalendarDays_CreatedById",
                table: "CalendarDays");

            migrationBuilder.DropIndex(
                name: "IX_CalendarDays_LastModifiedById",
                table: "CalendarDays");

            migrationBuilder.DropIndex(
                name: "IX_AuditLog_CreatedById",
                table: "AuditLog");

            migrationBuilder.DropIndex(
                name: "IX_AuditLog_LastModifiedById",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "WorkSite");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "WorkSite");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "WorkSchedule");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "WorkSchedule");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Leave");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Leave");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "DailyTransactionRecord");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "DailyTransactionRecord");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CalendarDays");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "CalendarDays");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "AuditLog");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WorkSite",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "WorkSite",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WorkSchedule",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "WorkSchedule",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Shift",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Shift",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "RawData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "RawData",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Leave",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Leave",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Holiday",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Holiday",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "GlobalSetting",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "GlobalSetting",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "ErrorLog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "ErrorLog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "DailyTransactionRecord",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "DailyTransactionRecord",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Calendars",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Calendars",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CalendarDays",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "CalendarDays",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WorkSite");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "WorkSite");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WorkSchedule");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "WorkSchedule");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Leave");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Leave");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "DailyTransactionRecord");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "DailyTransactionRecord");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CalendarDays");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "CalendarDays");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "WorkSite",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "WorkSite",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "WorkSchedule",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "WorkSchedule",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Shift",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "Shift",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "RawData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "RawData",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Leave",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "Leave",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Holiday",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "Holiday",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "GlobalSetting",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "GlobalSetting",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "ErrorLog",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "ErrorLog",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "Employee",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "DailyTransactionRecord",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "DailyTransactionRecord",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Calendars",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "Calendars",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "CalendarDays",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "CalendarDays",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "AuditLog",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AuditLog",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AuditLog",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "AuditLog",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "AuditLog",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkSite_CreatedById",
                table: "WorkSite",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSite_LastModifiedById",
                table: "WorkSite",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedule_CreatedById",
                table: "WorkSchedule",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedule_LastModifiedById",
                table: "WorkSchedule",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_CreatedById",
                table: "Shift",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_LastModifiedById",
                table: "Shift",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_RawData_CreatedById",
                table: "RawData",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RawData_LastModifiedById",
                table: "RawData",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Leave_CreatedById",
                table: "Leave",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Leave_LastModifiedById",
                table: "Leave",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_CreatedById",
                table: "Holiday",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_LastModifiedById",
                table: "Holiday",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalSetting_CreatedById",
                table: "GlobalSetting",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalSetting_LastModifiedById",
                table: "GlobalSetting",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLog_CreatedById",
                table: "ErrorLog",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLog_LastModifiedById",
                table: "ErrorLog",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_CreatedById",
                table: "Employee",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_LastModifiedById",
                table: "Employee",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTransactionRecord_CreatedById",
                table: "DailyTransactionRecord",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTransactionRecord_LastModifiedById",
                table: "DailyTransactionRecord",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_CreatedById",
                table: "Calendars",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_LastModifiedById",
                table: "Calendars",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarDays_CreatedById",
                table: "CalendarDays",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarDays_LastModifiedById",
                table: "CalendarDays",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_CreatedById",
                table: "AuditLog",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLog_LastModifiedById",
                table: "AuditLog",
                column: "LastModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLog_User_CreatedById",
                table: "AuditLog",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditLog_User_LastModifiedById",
                table: "AuditLog",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarDays_User_CreatedById",
                table: "CalendarDays",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarDays_User_LastModifiedById",
                table: "CalendarDays",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Calendars_User_CreatedById",
                table: "Calendars",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Calendars_User_LastModifiedById",
                table: "Calendars",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTransactionRecord_User_CreatedById",
                table: "DailyTransactionRecord",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTransactionRecord_User_LastModifiedById",
                table: "DailyTransactionRecord",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_User_CreatedById",
                table: "Employee",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_User_LastModifiedById",
                table: "Employee",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorLog_User_CreatedById",
                table: "ErrorLog",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorLog_User_LastModifiedById",
                table: "ErrorLog",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalSetting_User_CreatedById",
                table: "GlobalSetting",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalSetting_User_LastModifiedById",
                table: "GlobalSetting",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_User_CreatedById",
                table: "Holiday",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_User_LastModifiedById",
                table: "Holiday",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leave_User_CreatedById",
                table: "Leave",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Leave_User_LastModifiedById",
                table: "Leave",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_User_CreatedById",
                table: "Shift",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Shift_User_LastModifiedById",
                table: "Shift",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedule_User_CreatedById",
                table: "WorkSchedule",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedule_User_LastModifiedById",
                table: "WorkSchedule",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSite_User_CreatedById",
                table: "WorkSite",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSite_User_LastModifiedById",
                table: "WorkSite",
                column: "LastModifiedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
