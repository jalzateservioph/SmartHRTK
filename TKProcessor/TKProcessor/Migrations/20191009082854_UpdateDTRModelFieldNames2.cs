using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateDTRModelFieldNames2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApprovedNDspeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDSpeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDspeHolRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDSpeHolRD");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDspeHolOt",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDSpeHolOt");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDspeHol",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDSpeHol");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegSpeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDLegSpeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegSpeHolRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDLegSpeHolRD");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegSpeHolOt",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDLegSpeHolOt");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegSpeHol",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDLegSpeHol");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegHolRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDLegHolRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegHolRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDLegHolRD");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegHolOt",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDLegHolOt");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegHol",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDLegHol");

            migrationBuilder.RenameColumn(
                name: "ActualNDspeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ActualNDSpeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ActualNDspeHolRD",
                table: "DailyTransactionRecord",
                newName: "ActualNDSpeHolRD");

            migrationBuilder.RenameColumn(
                name: "ActualNDspeHolOt",
                table: "DailyTransactionRecord",
                newName: "ActualNDSpeHolOt");

            migrationBuilder.RenameColumn(
                name: "ActualNDspeHol",
                table: "DailyTransactionRecord",
                newName: "ActualNDSpeHol");

            migrationBuilder.RenameColumn(
                name: "ActualNDlegSpeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ActualNDLegSpeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ActualNDlegSpeHolRD",
                table: "DailyTransactionRecord",
                newName: "ActualNDLegSpeHolRD");

            migrationBuilder.RenameColumn(
                name: "ActualNDlegSpeHolOt",
                table: "DailyTransactionRecord",
                newName: "ActualNDLegSpeHolOt");

            migrationBuilder.RenameColumn(
                name: "ActualNDlegSpeHol",
                table: "DailyTransactionRecord",
                newName: "ActualNDLegSpeHol");

            migrationBuilder.RenameColumn(
                name: "ActualNDlegHolRD",
                table: "DailyTransactionRecord",
                newName: "ActualNDLegHolRD");

            migrationBuilder.RenameColumn(
                name: "ActualNDlegHol",
                table: "DailyTransactionRecord",
                newName: "ActualNDLegHol");

            migrationBuilder.RenameColumn(
                name: "ActualLegNDholRDot",
                table: "DailyTransactionRecord",
                newName: "ActualLegNDHolRDot");

            migrationBuilder.RenameColumn(
                name: "ActualLegNDholOt",
                table: "DailyTransactionRecord",
                newName: "ActualLegNDHolOt");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualRestDay",
                table: "DailyTransactionRecord",
                nullable: true,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApprovedNDSpeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDspeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDSpeHolRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDspeHolRD");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDSpeHolOt",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDspeHolOt");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDSpeHol",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDspeHol");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDLegSpeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegSpeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDLegSpeHolRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegSpeHolRD");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDLegSpeHolOt",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegSpeHolOt");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDLegSpeHol",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegSpeHol");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDLegHolRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegHolRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDLegHolRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegHolRD");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDLegHolOt",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegHolOt");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDLegHol",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegHol");

            migrationBuilder.RenameColumn(
                name: "ActualNDSpeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ActualNDspeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ActualNDSpeHolRD",
                table: "DailyTransactionRecord",
                newName: "ActualNDspeHolRD");

            migrationBuilder.RenameColumn(
                name: "ActualNDSpeHolOt",
                table: "DailyTransactionRecord",
                newName: "ActualNDspeHolOt");

            migrationBuilder.RenameColumn(
                name: "ActualNDSpeHol",
                table: "DailyTransactionRecord",
                newName: "ActualNDspeHol");

            migrationBuilder.RenameColumn(
                name: "ActualNDLegSpeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ActualNDlegSpeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ActualNDLegSpeHolRD",
                table: "DailyTransactionRecord",
                newName: "ActualNDlegSpeHolRD");

            migrationBuilder.RenameColumn(
                name: "ActualNDLegSpeHolOt",
                table: "DailyTransactionRecord",
                newName: "ActualNDlegSpeHolOt");

            migrationBuilder.RenameColumn(
                name: "ActualNDLegSpeHol",
                table: "DailyTransactionRecord",
                newName: "ActualNDlegSpeHol");

            migrationBuilder.RenameColumn(
                name: "ActualNDLegHolRD",
                table: "DailyTransactionRecord",
                newName: "ActualNDlegHolRD");

            migrationBuilder.RenameColumn(
                name: "ActualNDLegHol",
                table: "DailyTransactionRecord",
                newName: "ActualNDlegHol");

            migrationBuilder.RenameColumn(
                name: "ActualLegNDHolRDot",
                table: "DailyTransactionRecord",
                newName: "ActualLegNDholRDot");

            migrationBuilder.RenameColumn(
                name: "ActualLegNDHolOt",
                table: "DailyTransactionRecord",
                newName: "ActualLegNDholOt");

            migrationBuilder.AlterColumn<decimal>(
                name: "ActualRestDay",
                table: "DailyTransactionRecord",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }
    }
}
