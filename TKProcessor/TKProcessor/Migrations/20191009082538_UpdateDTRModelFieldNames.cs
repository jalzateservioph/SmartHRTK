using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateDTRModelFieldNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApprovedSpeHolRdot",
                table: "DailyTransactionRecord",
                newName: "ApprovedSpeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedSpeHolRd",
                table: "DailyTransactionRecord",
                newName: "ApprovedSpeHolRD");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdspeHolRdot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDspeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdspeHolRd",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDspeHolRD");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdspeHolOt",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDspeHolOt");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdspeHol",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDspeHol");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdrdot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdrd",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDRD");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdlegSpeHolRdot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegSpeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdlegSpeHolRd",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegSpeHolRD");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdlegSpeHolOt",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegSpeHolOt");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdlegSpeHol",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegSpeHol");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdlegHolRdot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegHolRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdlegHolRd",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegHolRD");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdlegHolOt",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegHolOt");

            migrationBuilder.RenameColumn(
                name: "ApprovedNdlegHol",
                table: "DailyTransactionRecord",
                newName: "ApprovedNDlegHol");

            migrationBuilder.RenameColumn(
                name: "ApprovedLegSpeHolRdot",
                table: "DailyTransactionRecord",
                newName: "ApprovedLegSpeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedLegSpeHolRd",
                table: "DailyTransactionRecord",
                newName: "ApprovedLegSpeHolRD");

            migrationBuilder.RenameColumn(
                name: "ApprovedLegHolRdot",
                table: "DailyTransactionRecord",
                newName: "ApprovedLegHolRDot");

            migrationBuilder.RenameColumn(
                name: "ApprovedLegHolRd",
                table: "DailyTransactionRecord",
                newName: "ApprovedLegHolRD");

            migrationBuilder.RenameColumn(
                name: "ActualSpeHolRdot",
                table: "DailyTransactionRecord",
                newName: "ActualSpeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ActualSpeHolRd",
                table: "DailyTransactionRecord",
                newName: "ActualSpeHolRD");

            migrationBuilder.RenameColumn(
                name: "ActualNdspeHolRdot",
                table: "DailyTransactionRecord",
                newName: "ActualNDspeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ActualNdspeHolRd",
                table: "DailyTransactionRecord",
                newName: "ActualNDspeHolRD");

            migrationBuilder.RenameColumn(
                name: "ActualNdspeHolOt",
                table: "DailyTransactionRecord",
                newName: "ActualNDspeHolOt");

            migrationBuilder.RenameColumn(
                name: "ActualNdspeHol",
                table: "DailyTransactionRecord",
                newName: "ActualNDspeHol");

            migrationBuilder.RenameColumn(
                name: "ActualNdrdot",
                table: "DailyTransactionRecord",
                newName: "ActualNDRDot");

            migrationBuilder.RenameColumn(
                name: "ActualNdrd",
                table: "DailyTransactionRecord",
                newName: "ActualNDRD");

            migrationBuilder.RenameColumn(
                name: "ActualNdlegSpeHolRdot",
                table: "DailyTransactionRecord",
                newName: "ActualNDlegSpeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ActualNdlegSpeHolRd",
                table: "DailyTransactionRecord",
                newName: "ActualNDlegSpeHolRD");

            migrationBuilder.RenameColumn(
                name: "ActualNdlegSpeHolOt",
                table: "DailyTransactionRecord",
                newName: "ActualNDlegSpeHolOt");

            migrationBuilder.RenameColumn(
                name: "ActualNdlegSpeHol",
                table: "DailyTransactionRecord",
                newName: "ActualNDlegSpeHol");

            migrationBuilder.RenameColumn(
                name: "ActualNdlegHolRd",
                table: "DailyTransactionRecord",
                newName: "ActualNDlegHolRD");

            migrationBuilder.RenameColumn(
                name: "ActualNdlegHol",
                table: "DailyTransactionRecord",
                newName: "ActualNDlegHol");

            migrationBuilder.RenameColumn(
                name: "ActualLegSpeHolRdot",
                table: "DailyTransactionRecord",
                newName: "ActualLegSpeHolRDot");

            migrationBuilder.RenameColumn(
                name: "ActualLegSpeHolRd",
                table: "DailyTransactionRecord",
                newName: "ActualLegSpeHolRD");

            migrationBuilder.RenameColumn(
                name: "ActualLegNdholRdot",
                table: "DailyTransactionRecord",
                newName: "ActualLegNDholRDot");

            migrationBuilder.RenameColumn(
                name: "ActualLegNdholOt",
                table: "DailyTransactionRecord",
                newName: "ActualLegNDholOt");

            migrationBuilder.RenameColumn(
                name: "ActualLegHolRdot",
                table: "DailyTransactionRecord",
                newName: "ActualLegHolRDot");

            migrationBuilder.RenameColumn(
                name: "ActualLegHolRd",
                table: "DailyTransactionRecord",
                newName: "ActualLegHolRD");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApprovedSpeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedSpeHolRdot");

            migrationBuilder.RenameColumn(
                name: "ApprovedSpeHolRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedSpeHolRd");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDspeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdspeHolRdot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDspeHolRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdspeHolRd");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDspeHolOt",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdspeHolOt");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDspeHol",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdspeHol");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegSpeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdlegSpeHolRdot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegSpeHolRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdlegSpeHolRd");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegSpeHolOt",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdlegSpeHolOt");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegSpeHol",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdlegSpeHol");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegHolRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdlegHolRdot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegHolRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdlegHolRd");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegHolOt",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdlegHolOt");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDlegHol",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdlegHol");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdrdot");

            migrationBuilder.RenameColumn(
                name: "ApprovedNDRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedNdrd");

            migrationBuilder.RenameColumn(
                name: "ApprovedLegSpeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedLegSpeHolRdot");

            migrationBuilder.RenameColumn(
                name: "ApprovedLegSpeHolRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedLegSpeHolRd");

            migrationBuilder.RenameColumn(
                name: "ApprovedLegHolRDot",
                table: "DailyTransactionRecord",
                newName: "ApprovedLegHolRdot");

            migrationBuilder.RenameColumn(
                name: "ApprovedLegHolRD",
                table: "DailyTransactionRecord",
                newName: "ApprovedLegHolRd");

            migrationBuilder.RenameColumn(
                name: "ActualSpeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ActualSpeHolRdot");

            migrationBuilder.RenameColumn(
                name: "ActualSpeHolRD",
                table: "DailyTransactionRecord",
                newName: "ActualSpeHolRd");

            migrationBuilder.RenameColumn(
                name: "ActualNDspeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ActualNdspeHolRdot");

            migrationBuilder.RenameColumn(
                name: "ActualNDspeHolRD",
                table: "DailyTransactionRecord",
                newName: "ActualNdspeHolRd");

            migrationBuilder.RenameColumn(
                name: "ActualNDspeHolOt",
                table: "DailyTransactionRecord",
                newName: "ActualNdspeHolOt");

            migrationBuilder.RenameColumn(
                name: "ActualNDspeHol",
                table: "DailyTransactionRecord",
                newName: "ActualNdspeHol");

            migrationBuilder.RenameColumn(
                name: "ActualNDlegSpeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ActualNdlegSpeHolRdot");

            migrationBuilder.RenameColumn(
                name: "ActualNDlegSpeHolRD",
                table: "DailyTransactionRecord",
                newName: "ActualNdlegSpeHolRd");

            migrationBuilder.RenameColumn(
                name: "ActualNDlegSpeHolOt",
                table: "DailyTransactionRecord",
                newName: "ActualNdlegSpeHolOt");

            migrationBuilder.RenameColumn(
                name: "ActualNDlegSpeHol",
                table: "DailyTransactionRecord",
                newName: "ActualNdlegSpeHol");

            migrationBuilder.RenameColumn(
                name: "ActualNDlegHolRD",
                table: "DailyTransactionRecord",
                newName: "ActualNdlegHolRd");

            migrationBuilder.RenameColumn(
                name: "ActualNDlegHol",
                table: "DailyTransactionRecord",
                newName: "ActualNdlegHol");

            migrationBuilder.RenameColumn(
                name: "ActualNDRDot",
                table: "DailyTransactionRecord",
                newName: "ActualNdrdot");

            migrationBuilder.RenameColumn(
                name: "ActualNDRD",
                table: "DailyTransactionRecord",
                newName: "ActualNdrd");

            migrationBuilder.RenameColumn(
                name: "ActualLegSpeHolRDot",
                table: "DailyTransactionRecord",
                newName: "ActualLegSpeHolRdot");

            migrationBuilder.RenameColumn(
                name: "ActualLegSpeHolRD",
                table: "DailyTransactionRecord",
                newName: "ActualLegSpeHolRd");

            migrationBuilder.RenameColumn(
                name: "ActualLegNDholRDot",
                table: "DailyTransactionRecord",
                newName: "ActualLegNdholRdot");

            migrationBuilder.RenameColumn(
                name: "ActualLegNDholOt",
                table: "DailyTransactionRecord",
                newName: "ActualLegNdholOt");

            migrationBuilder.RenameColumn(
                name: "ActualLegHolRDot",
                table: "DailyTransactionRecord",
                newName: "ActualLegHolRdot");

            migrationBuilder.RenameColumn(
                name: "ActualLegHolRD",
                table: "DailyTransactionRecord",
                newName: "ActualLegHolRd");
        }
    }
}
