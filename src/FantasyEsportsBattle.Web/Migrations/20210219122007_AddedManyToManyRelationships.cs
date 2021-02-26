using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Host.Migrations
{
    public partial class AddedManyToManyRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tournaments_TournamentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_Tournaments_TournamentId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_TournamentId",
                table: "Competitions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TournamentId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ApplicationUserTournaments",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TournamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserTournaments", x => new { x.TournamentId, x.ApplicationUserId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserTournaments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserTournaments_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TournamentCompetitions",
                columns: table => new
                {
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    CompetitionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentCompetitions", x => new { x.TournamentId, x.CompetitionId });
                    table.ForeignKey(
                        name: "FK_TournamentCompetitions_Competitions_CompetitionId",
                        column: x => x.CompetitionId,
                        principalTable: "Competitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentCompetitions_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserTournaments_ApplicationUserId",
                table: "ApplicationUserTournaments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentCompetitions_CompetitionId",
                table: "TournamentCompetitions",
                column: "CompetitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserTournaments");

            migrationBuilder.DropTable(
                name: "TournamentCompetitions");

            migrationBuilder.AddColumn<int>(
                name: "TournamentId",
                table: "Competitions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TournamentId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_TournamentId",
                table: "Competitions",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TournamentId",
                table: "AspNetUsers",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tournaments_TournamentId",
                table: "AspNetUsers",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_Tournaments_TournamentId",
                table: "Competitions",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
