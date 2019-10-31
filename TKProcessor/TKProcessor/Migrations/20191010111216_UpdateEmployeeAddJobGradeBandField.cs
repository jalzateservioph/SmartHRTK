using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateEmployeeAddJobGradeBandField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeCode",
                table: "DailyTransactionRecord");

            migrationBuilder.AddColumn<string>(
                name: "JobGradeBand",
                table: "Employee",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId",
                table: "DailyTransactionRecord",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DailyTransactionRecord_EmployeeId",
                table: "DailyTransactionRecord",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyTransactionRecord_Employee_EmployeeId",
                table: "DailyTransactionRecord",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyTransactionRecord_Employee_EmployeeId",
                table: "DailyTransactionRecord");

            migrationBuilder.DropIndex(
                name: "IX_DailyTransactionRecord_EmployeeId",
                table: "DailyTransactionRecord");

            migrationBuilder.DropColumn(
                name: "JobGradeBand",
                table: "Employee");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "DailyTransactionRecord");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeCode",
                table: "DailyTransactionRecord",
                nullable: true);
        }
    }
}
