using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Web.Migrations
{
    public partial class RenamedPlayerTablesTwo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPlayers_Teams_TeamId",
                table: "TournamentPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentPlayerStatuses_TournamentPlayers_CompetitionPlayerId",
                table: "TournamentPlayerStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentPlayerStatuses",
                table: "TournamentPlayerStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentPlayers",
                table: "TournamentPlayers");

            migrationBuilder.RenameTable(
                name: "TournamentPlayerStatuses",
                newName: "CompetitionPlayerStatuses");

            migrationBuilder.RenameTable(
                name: "TournamentPlayers",
                newName: "CompetitionPlayers");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentPlayerStatuses_CompetitionPlayerId",
                table: "CompetitionPlayerStatuses",
                newName: "IX_CompetitionPlayerStatuses_CompetitionPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentPlayers_TeamId",
                table: "CompetitionPlayers",
                newName: "IX_CompetitionPlayers_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompetitionPlayerStatuses",
                table: "CompetitionPlayerStatuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompetitionPlayers",
                table: "CompetitionPlayers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionPlayers_Teams_TeamId",
                table: "CompetitionPlayers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompetitionPlayerStatuses_CompetitionPlayers_CompetitionPlayerId",
                table: "CompetitionPlayerStatuses",
                column: "CompetitionPlayerId",
                principalTable: "CompetitionPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionPlayers_Teams_TeamId",
                table: "CompetitionPlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_CompetitionPlayerStatuses_CompetitionPlayers_CompetitionPlayerId",
                table: "CompetitionPlayerStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompetitionPlayerStatuses",
                table: "CompetitionPlayerStatuses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompetitionPlayers",
                table: "CompetitionPlayers");

            migrationBuilder.RenameTable(
                name: "CompetitionPlayerStatuses",
                newName: "TournamentPlayerStatuses");

            migrationBuilder.RenameTable(
                name: "CompetitionPlayers",
                newName: "TournamentPlayers");

            migrationBuilder.RenameIndex(
                name: "IX_CompetitionPlayerStatuses_CompetitionPlayerId",
                table: "TournamentPlayerStatuses",
                newName: "IX_TournamentPlayerStatuses_CompetitionPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_CompetitionPlayers_TeamId",
                table: "TournamentPlayers",
                newName: "IX_TournamentPlayers_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentPlayerStatuses",
                table: "TournamentPlayerStatuses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentPlayers",
                table: "TournamentPlayers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPlayers_Teams_TeamId",
                table: "TournamentPlayers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentPlayerStatuses_TournamentPlayers_CompetitionPlayerId",
                table: "TournamentPlayerStatuses",
                column: "CompetitionPlayerId",
                principalTable: "TournamentPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
