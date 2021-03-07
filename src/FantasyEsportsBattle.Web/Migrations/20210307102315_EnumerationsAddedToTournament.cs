using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Web.Migrations
{
    public partial class EnumerationsAddedToTournament : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TournamentAlgorithm",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TournamentType",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TournamentAlgorithm",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "TournamentType",
                table: "Tournaments");
        }
    }
}
