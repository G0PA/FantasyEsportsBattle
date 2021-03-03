using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Web.Migrations
{
    public partial class TournamentInvitationTableAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TournamentHostId",
                table: "Tournaments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TournamentInvitation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournamentId = table.Column<int>(type: "int", nullable: false),
                    InvitedUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InvitationSenderUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InvitationSenderId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentInvitation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentInvitation_AspNetUsers_InvitationSenderId",
                        column: x => x.InvitationSenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentInvitation_AspNetUsers_InvitedUserId",
                        column: x => x.InvitedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TournamentInvitation_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_TournamentHostId",
                table: "Tournaments",
                column: "TournamentHostId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentInvitation_InvitationSenderId",
                table: "TournamentInvitation",
                column: "InvitationSenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentInvitation_InvitedUserId",
                table: "TournamentInvitation",
                column: "InvitedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentInvitation_TournamentId",
                table: "TournamentInvitation",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournaments_AspNetUsers_TournamentHostId",
                table: "Tournaments",
                column: "TournamentHostId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournaments_AspNetUsers_TournamentHostId",
                table: "Tournaments");

            migrationBuilder.DropTable(
                name: "TournamentInvitation");

            migrationBuilder.DropIndex(
                name: "IX_Tournaments_TournamentHostId",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "TournamentHostId",
                table: "Tournaments");
        }
    }
}
