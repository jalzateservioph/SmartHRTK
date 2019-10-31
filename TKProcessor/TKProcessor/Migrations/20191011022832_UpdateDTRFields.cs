using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateDTRFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allowance",
                table: "DailyTransactionRecord");

            migrationBuilder.DropColumn(
                name: "ModifiedIo",
                table: "DailyTransactionRecord");

            migrationBuilder.AlterColumn<decimal>(
                name: "WorkHours",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "NightDifferentialOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "NightDifferential",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedUndertime",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedSpeHol",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedRestDayOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedRestDay",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedPreOvertime",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedPostOvertime",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedOvertime",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDSpeHol",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegSpeHol",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegHolRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegHolRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegHolOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegHol",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegSpeHol",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegHolRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegHolRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegHolOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegHol",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLate",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualUndertime",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualSpeHol",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualRestDayOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualRestDay",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualPreOvertime",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualPostOvertime",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualOvertime",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDSpeHol",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDLegSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDLegSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDLegSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDLegSpeHol",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDLegHolRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDLegHol",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegSpeHol",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegNDHolRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegNDHolOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegHolRDot",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegHolRD",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegHolOt",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegHol",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLate",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "AbsentHours",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "WorkHours",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "NightDifferentialOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "NightDifferential",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedUndertime",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedSpeHol",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedRestDayOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedRestDay",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedPreOvertime",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedPostOvertime",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedOvertime",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDSpeHol",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegSpeHol",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegHolRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegHolRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegHolOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedNDLegHol",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegSpeHol",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegHolRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegHolRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegHolOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLegHol",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ApprovedLate",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualUndertime",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualSpeHol",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualRestDayOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualRestDay",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualPreOvertime",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualPostOvertime",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualOvertime",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDSpeHol",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDLegSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDLegSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDLegSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDLegSpeHol",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDLegHolRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualNDLegHol",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegSpeHolRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegSpeHolRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegSpeHolOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegSpeHol",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegNDHolRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegNDHolOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegHolRDot",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegHolRD",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegHolOt",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLegHol",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualLate",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "AbsentHours",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<decimal>(
                name: "Allowance",
                table: "DailyTransactionRecord",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifiedIo",
                table: "DailyTransactionRecord",
                nullable: true);
        }
    }
}
