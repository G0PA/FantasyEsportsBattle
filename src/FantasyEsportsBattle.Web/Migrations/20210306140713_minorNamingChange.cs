using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Web.Migrations
{
    public partial class minorNamingChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentInvitation_AspNetUsers_InvitationSenderId",
                table: "TournamentInvitation");

            migrationBuilder.DropIndex(
                name: "IX_TournamentInvitation_InvitationSenderId",
                table: "TournamentInvitation");

            migrationBuilder.DropColumn(
                name: "InvitationSenderId",
                table: "TournamentInvitation");

            migrationBuilder.AlterColumn<string>(
                name: "InvitationSenderUserId",
                table: "TournamentInvitation",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentInvitation_InvitationSenderUserId",
                table: "TournamentInvitation",
                column: "InvitationSenderUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentInvitation_AspNetUsers_InvitationSenderUserId",
                table: "TournamentInvitation",
                column: "InvitationSenderUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentInvitation_AspNetUsers_InvitationSenderUserId",
                table: "TournamentInvitation");

            migrationBuilder.DropIndex(
                name: "IX_TournamentInvitation_InvitationSenderUserId",
                table: "TournamentInvitation");

            migrationBuilder.AlterColumn<string>(
                name: "InvitationSenderUserId",
                table: "TournamentInvitation",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvitationSenderId",
                table: "TournamentInvitation",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentInvitation_InvitationSenderId",
                table: "TournamentInvitation",
                column: "InvitationSenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentInvitation_AspNetUsers_InvitationSenderId",
                table: "TournamentInvitation",
                column: "InvitationSenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
