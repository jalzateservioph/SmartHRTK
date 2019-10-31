using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateDTRRenameNDLEGHOLOT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActualLegNDHolOt",
                table: "DailyTransactionRecord",
                newName: "ActualNDLegHolOt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActualNDLegHolOt",
                table: "DailyTransactionRecord",
                newName: "ActualLegNDHolOt");
        }
    }
}
