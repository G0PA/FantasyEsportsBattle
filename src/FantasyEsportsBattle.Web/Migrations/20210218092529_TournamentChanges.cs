using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Host.Migrations
{
    public partial class TournamentChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
