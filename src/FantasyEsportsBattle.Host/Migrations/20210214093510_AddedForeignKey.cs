using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Host.Migrations
{
    public partial class AddedForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPlayers_Teams_TeamId",
                table: "TournamentPlayers");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "TournamentPlayers");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "TournamentPlayers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPlayers_Teams_TeamId",
                table: "TournamentPlayers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPlayers_Teams_TeamId",
                table: "TournamentPlayers");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "TournamentPlayers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "TournamentPlayers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPlayers_Teams_TeamId",
                table: "TournamentPlayers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
