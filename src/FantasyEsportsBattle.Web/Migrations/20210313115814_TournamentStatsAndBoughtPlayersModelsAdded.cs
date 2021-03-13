using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Web.Migrations
{
    public partial class TournamentStatsAndBoughtPlayersModelsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TournamentStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Currency = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentStatuses_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentStatuses_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TournamentBoughtPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentStatsId = table.Column<int>(type: "int", nullable: false),
                    CompetitionPlayerId = table.Column<int>(type: "int", nullable: false),
                    IsReserve = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentBoughtPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentBoughtPlayers_CompetitionPlayers_CompetitionPlayerId",
                        column: x => x.CompetitionPlayerId,
                        principalTable: "CompetitionPlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TournamentBoughtPlayers_TournamentStatuses_TournamentStatsId",
                        column: x => x.TournamentStatsId,
                        principalTable: "TournamentStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentBoughtPlayers_CompetitionPlayerId",
                table: "TournamentBoughtPlayers",
                column: "CompetitionPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentBoughtPlayers_TournamentStatsId",
                table: "TournamentBoughtPlayers",
                column: "TournamentStatsId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentStatuses_ApplicationUserId",
                table: "TournamentStatuses",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentStatuses_TournamentId",
                table: "TournamentStatuses",
                column: "TournamentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TournamentBoughtPlayers");

            migrationBuilder.DropTable(
                name: "TournamentStatuses");
        }
    }
}
