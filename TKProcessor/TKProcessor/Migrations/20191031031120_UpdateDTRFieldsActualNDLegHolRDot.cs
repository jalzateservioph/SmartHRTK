using Microsoft.EntityFrameworkCore.Migrations;

namespace TKProcessor.Migrations
{
    public partial class UpdateDTRFieldsActualNDLegHolRDot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActualLegNDHolRDot",
                table: "DailyTransactionRecord",
                newName: "ActualNDLegHolRDot");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActualNDLegHolRDot",
                table: "DailyTransactionRecord",
                newName: "ActualLegNDHolRDot");
        }
    }
}
