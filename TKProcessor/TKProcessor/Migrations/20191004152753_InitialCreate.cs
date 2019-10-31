using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Target = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModifiedOn = table.Column<DateTime>(nullable: false),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLog", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Source = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    StackTrace = table.Column<string>(nullable: true),
                    DateRaised = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GlobalSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Leave",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmployeeCode = table.Column<string>(nullable: true),
                    LeaveDate = table.Column<DateTime>(nullable: false),
                    LeaveType = table.Column<string>(nullable: true),
                    LeaveHours = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leave", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DailyTransactionRecord",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmployeeCode = table.Column<string>(nullable: true),
                    ShiftCode = table.Column<string>(nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: true),
                    TimeIn = table.Column<DateTime>(nullable: true),
                    TimeOut = table.Column<DateTime>(nullable: true),
                    WorkHours = table.Column<decimal>(nullable: true),
                    AbsentHours = table.Column<decimal>(nullable: true),
                    ActualLate = table.Column<decimal>(nullable: true),
                    ApprovedLate = table.Column<decimal>(nullable: true),
                    ActualUndertime = table.Column<decimal>(nullable: true),
                    ApprovedUndertime = table.Column<decimal>(nullable: true),
                    ActualOvertime = table.Column<decimal>(nullable: true),
                    ApprovedOvertime = table.Column<decimal>(nullable: true),
                    ActualPreOvertime = table.Column<decimal>(nullable: true),
                    ApprovedPreOvertime = table.Column<decimal>(nullable: true),
                    ActualPostOvertime = table.Column<decimal>(nullable: true),
                    ApprovedPostOvertime = table.Column<decimal>(nullable: true),
                    NightDifferential = table.Column<decimal>(nullable: true),
                    NightDifferentialOt = table.Column<decimal>(nullable: true),
                    ActualRestDay = table.Column<decimal>(nullable: false),
                    ApprovedRestDay = table.Column<decimal>(nullable: true),
                    ActualRestDayOt = table.Column<decimal>(nullable: true),
                    ApprovedRestDayOt = table.Column<decimal>(nullable: true),
                    ActualNdrd = table.Column<decimal>(nullable: true),
                    ApprovedNdrd = table.Column<decimal>(nullable: true),
                    ActualNdrdot = table.Column<decimal>(nullable: true),
                    ApprovedNdrdot = table.Column<decimal>(nullable: true),
                    ActualLegHol = table.Column<decimal>(nullable: true),
                    ApprovedLegHol = table.Column<decimal>(nullable: true),
                    ActualLegHolOt = table.Column<decimal>(nullable: true),
                    ApprovedLegHolOt = table.Column<decimal>(nullable: true),
                    ActualSpeHol = table.Column<decimal>(nullable: true),
                    ApprovedSpeHol = table.Column<decimal>(nullable: true),
                    ActualSpeHolOt = table.Column<decimal>(nullable: true),
                    ApprovedSpeHolOt = table.Column<decimal>(nullable: true),
                    ActualLegSpeHol = table.Column<decimal>(nullable: true),
                    ApprovedLegSpeHol = table.Column<decimal>(nullable: true),
                    ActualLegSpeHolOt = table.Column<decimal>(nullable: true),
                    ApprovedLegSpeHolOt = table.Column<decimal>(nullable: true),
                    ActualLegHolRd = table.Column<decimal>(nullable: true),
                    ApprovedLegHolRd = table.Column<decimal>(nullable: true),
                    ActualLegHolRdot = table.Column<decimal>(nullable: true),
                    ApprovedLegHolRdot = table.Column<decimal>(nullable: true),
                    ActualSpeHolRd = table.Column<decimal>(nullable: true),
                    ApprovedSpeHolRd = table.Column<decimal>(nullable: true),
                    ActualSpeHolRdot = table.Column<decimal>(nullable: true),
                    ApprovedSpeHolRdot = table.Column<decimal>(nullable: true),
                    ActualLegSpeHolRd = table.Column<decimal>(nullable: true),
                    ApprovedLegSpeHolRd = table.Column<decimal>(nullable: true),
                    ActualLegSpeHolRdot = table.Column<decimal>(nullable: true),
                    ApprovedLegSpeHolRdot = table.Column<decimal>(nullable: true),
                    ActualNdlegHol = table.Column<decimal>(nullable: true),
                    ApprovedNdlegHol = table.Column<decimal>(nullable: true),
                    ActualLegNdholOt = table.Column<decimal>(nullable: true),
                    ApprovedNdlegHolOt = table.Column<decimal>(nullable: true),
                    ActualNdspeHol = table.Column<decimal>(nullable: true),
                    ApprovedNdspeHol = table.Column<decimal>(nullable: true),
                    ActualNdspeHolOt = table.Column<decimal>(nullable: true),
                    ApprovedNdspeHolOt = table.Column<decimal>(nullable: true),
                    ActualNdlegSpeHol = table.Column<decimal>(nullable: true),
                    ApprovedNdlegSpeHol = table.Column<decimal>(nullable: true),
                    ActualNdlegSpeHolOt = table.Column<decimal>(nullable: true),
                    ApprovedNdlegSpeHolOt = table.Column<decimal>(nullable: true),
                    ActualNdlegHolRd = table.Column<decimal>(nullable: true),
                    ApprovedNdlegHolRd = table.Column<decimal>(nullable: true),
                    ActualLegNdholRdot = table.Column<decimal>(nullable: true),
                    ApprovedNdlegHolRdot = table.Column<decimal>(nullable: true),
                    ActualNdspeHolRd = table.Column<decimal>(nullable: true),
                    ApprovedNdspeHolRd = table.Column<decimal>(nullable: true),
                    ActualNdspeHolRdot = table.Column<decimal>(nullable: true),
                    ApprovedNdspeHolRdot = table.Column<decimal>(nullable: true),
                    ActualNdlegSpeHolRd = table.Column<decimal>(nullable: true),
                    ApprovedNdlegSpeHolRd = table.Column<decimal>(nullable: true),
                    ActualNdlegSpeHolRdot = table.Column<decimal>(nullable: true),
                    ApprovedNdlegSpeHolRdot = table.Column<decimal>(nullable: true),
                    Allowance = table.Column<decimal>(nullable: true),
                    Remarks = table.Column<string>(nullable: true),
                    LeaveType = table.Column<string>(nullable: true),
                    ModifiedIo = table.Column<string>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedById = table.Column<Guid>(nullable: true),
                    LastModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyTransactionRecord", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyTransactionRecord_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DailyTransactionRecord_User_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmployeeCode = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    BiometricsId = table.Column<string>(nullable: true),
                    TerminationDate = table.Column<DateTime>(nullable: true),
                    ShiftCode = table.Column<string>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedById = table.Column<Guid>(nullable: true),
                    LastModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employee_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employee_User_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Holiday",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    HolidayName = table.Column<string>(nullable: true),
                    HolidayType = table.Column<string>(nullable: true),
                    HolidayDate = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedById = table.Column<Guid>(nullable: true),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    CalendarId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holiday_Calendar_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Holiday_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Holiday_User_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RawData",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BiometricsId = table.Column<string>(nullable: true),
                    TransactionType = table.Column<int>(nullable: true),
                    ScheduleDate = table.Column<DateTime>(nullable: true),
                    TransactionDateTime = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedById = table.Column<Guid>(nullable: true),
                    LastModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RawData_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RawData_User_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shift",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ShiftCode = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ShiftType = table.Column<int>(nullable: true),
                    FocusDate = table.Column<int>(nullable: true),
                    ScheduleIn = table.Column<DateTime>(nullable: true),
                    ScheduleOut = table.Column<DateTime>(nullable: true),
                    RequiredWorkHours = table.Column<decimal>(nullable: true),
                    IsRestDay = table.Column<bool>(nullable: true),
                    AmbreakIn = table.Column<DateTime>(nullable: true),
                    AmbreakOut = table.Column<DateTime>(nullable: true),
                    PmbreakIn = table.Column<DateTime>(nullable: true),
                    PmbreakOut = table.Column<DateTime>(nullable: true),
                    LunchIn = table.Column<DateTime>(nullable: true),
                    LunchOut = table.Column<DateTime>(nullable: true),
                    DinnerIn = table.Column<DateTime>(nullable: true),
                    DinnerOut = table.Column<DateTime>(nullable: true),
                    IsLateIn = table.Column<bool>(nullable: true),
                    GracePeriodLateIn = table.Column<int>(nullable: true),
                    AfterEvery = table.Column<int>(nullable: true),
                    DeductionLateIn = table.Column<int>(nullable: true),
                    IsPlusLateInMinutes = table.Column<bool>(nullable: true),
                    MaximumMinutesConsideredAsHalfDay = table.Column<int>(nullable: true),
                    IsEarlyOut = table.Column<bool>(nullable: true),
                    GracePeriodEarlyOut = table.Column<int>(nullable: true),
                    AfterEveryEarlyOut = table.Column<decimal>(nullable: true),
                    DeductionOfEarlyOut = table.Column<int>(nullable: true),
                    IsPlusEarlyOutMinutes = table.Column<bool>(nullable: true),
                    MaximumMinutesConsideredAsHalfAayEarlyOut = table.Column<int>(nullable: true),
                    IsPreShiftOt = table.Column<bool>(nullable: true),
                    MinimumPreShiftOt = table.Column<int>(nullable: true),
                    MaximumPreShiftOt = table.Column<int>(nullable: true),
                    RoundPreShiftOt = table.Column<int>(nullable: true),
                    IsPostShiftOt = table.Column<bool>(nullable: true),
                    MinimumPostShiftOt = table.Column<int>(nullable: true),
                    MaximumPostShiftOt = table.Column<int>(nullable: true),
                    RoundPostShiftOt = table.Column<int>(nullable: true),
                    IsHolidayRestDayOt = table.Column<bool>(nullable: true),
                    MinimumHolidayRestDayOt = table.Column<int>(nullable: true),
                    MaximumHolidayRestDayOt = table.Column<int>(nullable: true),
                    RoundHolidayRestDayOt = table.Column<int>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedById = table.Column<Guid>(nullable: true),
                    LastModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shift_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Shift_User_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EmployeeCode = table.Column<string>(nullable: true),
                    ScheduleDate = table.Column<DateTime>(nullable: false),
                    ShiftCode = table.Column<string>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: true),
                    LastModifiedById = table.Column<Guid>(nullable: true),
                    LastModifiedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkSchedule_User_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkSchedule_User_LastModifiedById",
                        column: x => x.LastModifiedById,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DailyTransactionRecord_CreatedById",
                table: "DailyTransactionRecord",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DailyTransactionRecord_LastModifiedById",
                table: "DailyTransactionRecord",
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
                name: "IX_Holiday_CalendarId",
                table: "Holiday",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_CreatedById",
                table: "Holiday",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_LastModifiedById",
                table: "Holiday",
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
                name: "IX_Shift_CreatedById",
                table: "Shift",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Shift_LastModifiedById",
                table: "Shift",
                column: "LastModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedule_CreatedById",
                table: "WorkSchedule",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedule_LastModifiedById",
                table: "WorkSchedule",
                column: "LastModifiedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropTable(
                name: "DailyTransactionRecord");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "ErrorLog");

            migrationBuilder.DropTable(
                name: "GlobalSetting");

            migrationBuilder.DropTable(
                name: "Holiday");

            migrationBuilder.DropTable(
                name: "Leave");

            migrationBuilder.DropTable(
                name: "RawData");

            migrationBuilder.DropTable(
                name: "Shift");

            migrationBuilder.DropTable(
                name: "WorkSchedule");

            migrationBuilder.DropTable(
                name: "Calendar");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
