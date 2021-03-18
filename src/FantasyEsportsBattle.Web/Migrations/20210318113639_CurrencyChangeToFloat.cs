using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Web.Migrations
{
    public partial class CurrencyChangeToFloat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Currency",
                table: "TournamentStatuses",
                type: "real",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Currency",
                table: "TournamentStatuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");
        }
    }
}
