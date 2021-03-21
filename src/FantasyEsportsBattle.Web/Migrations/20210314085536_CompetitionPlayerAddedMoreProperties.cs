using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Web.Migrations
{
    public partial class CompetitionPlayerAddedMoreProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "AheadInCSAt15MinPercent",
                table: "CompetitionPlayers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "CSDifferenceAt15Min",
                table: "CompetitionPlayers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "CSPM",
                table: "CompetitionPlayers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DamagePerMinute",
                table: "CompetitionPlayers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DamagePercent",
                table: "CompetitionPlayers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FirstBloodParticipationPercent",
                table: "CompetitionPlayers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "FirstBloodVictimPercent",
                table: "CompetitionPlayers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "GPM",
                table: "CompetitionPlayers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "GoldDifferenceAt15Min",
                table: "CompetitionPlayers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "GoldPercent",
                table: "CompetitionPlayers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "KDA",
                table: "CompetitionPlayers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "KillParticipationPercent",
                table: "CompetitionPlayers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Losses",
                table: "CompetitionPlayers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "VisionScorePerMinute",
                table: "CompetitionPlayers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Winrate",
                table: "CompetitionPlayers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "Wins",
                table: "CompetitionPlayers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "XPDifferenceAt15Min",
                table: "CompetitionPlayers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AheadInCSAt15MinPercent",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "CSDifferenceAt15Min",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "CSPM",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "DamagePerMinute",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "DamagePercent",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "FirstBloodParticipationPercent",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "FirstBloodVictimPercent",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "GPM",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "GoldDifferenceAt15Min",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "GoldPercent",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "KDA",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "KillParticipationPercent",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "Losses",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "VisionScorePerMinute",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "Winrate",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "Wins",
                table: "CompetitionPlayers");

            migrationBuilder.DropColumn(
                name: "XPDifferenceAt15Min",
                table: "CompetitionPlayers");
        }
    }
}
