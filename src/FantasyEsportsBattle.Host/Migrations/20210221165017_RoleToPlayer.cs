using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Host.Migrations
{
    public partial class RoleToPlayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "TournamentPlayers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "TournamentPlayers");
        }
    }
}
