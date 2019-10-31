using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateModelsAddFiedIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Holiday_Calendar_CalendarId",
                table: "Holiday");

            migrationBuilder.DropTable(
                name: "Calendar");

            migrationBuilder.DropIndex(
                name: "IX_Holiday_CalendarId",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "CalendarId",
                table: "Holiday");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "WorkSchedule",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "User",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "User",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Shift",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "RawData",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "RawData",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RawData",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "RawData",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "RawData",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Leave",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Leave",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Leave",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "Leave",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "Leave",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Holiday",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "GlobalSetting",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "GlobalSetting",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "GlobalSetting",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "GlobalSetting",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "GlobalSetting",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "ErrorLog",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ErrorLog",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ErrorLog",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "ErrorLog",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "ErrorLog",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Employee",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "DailyTransactionRecord",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "AuditLog",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "AuditLog",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AuditLog",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "AuditLog",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "AuditLog",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedById",
                table: "User",
                column: "CreatedById",
                unique: true,
                filter: "[CreatedById] IS NOT NULL");

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
                name: "FK_User_User_CreatedById",
                table: "User",
                column: "CreatedById",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditLog_User_CreatedById",
                table: "AuditLog");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditLog_User_LastModifiedById",
                table: "AuditLog");

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
                name: "FK_User_User_CreatedById",
                table: "User");

            migrationBuilder.DropIndex(
                name: "IX_User_CreatedById",
                table: "User");

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
                name: "IX_AuditLog_CreatedById",
                table: "AuditLog");

            migrationBuilder.DropIndex(
                name: "IX_AuditLog_LastModifiedById",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "WorkSchedule");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "RawData");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Leave");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Leave");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Leave");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "Leave");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "Leave");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Holiday");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "GlobalSetting");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "ErrorLog");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "DailyTransactionRecord");

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

            migrationBuilder.AddColumn<Guid>(
                name: "CalendarId",
                table: "Holiday",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Calendar",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendar", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_CalendarId",
                table: "Holiday",
                column: "CalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Holiday_Calendar_CalendarId",
                table: "Holiday",
                column: "CalendarId",
                principalTable: "Calendar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
