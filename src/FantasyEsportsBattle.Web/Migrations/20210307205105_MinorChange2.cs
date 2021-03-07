using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Web.Migrations
{
    public partial class MinorChange2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentInvitation_AspNetUsers_InvitationSenderUserId",
                table: "TournamentInvitation");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentInvitation_AspNetUsers_InvitedUserId",
                table: "TournamentInvitation");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentInvitation_Tournaments_TournamentId",
                table: "TournamentInvitation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentInvitation",
                table: "TournamentInvitation");

            migrationBuilder.RenameTable(
                name: "TournamentInvitation",
                newName: "TournamentInvitations");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentInvitation_TournamentId",
                table: "TournamentInvitations",
                newName: "IX_TournamentInvitations_TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentInvitation_InvitedUserId",
                table: "TournamentInvitations",
                newName: "IX_TournamentInvitations_InvitedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentInvitation_InvitationSenderUserId",
                table: "TournamentInvitations",
                newName: "IX_TournamentInvitations_InvitationSenderUserId");

            migrationBuilder.AddColumn<int>(
                name: "TournamentState",
                table: "Tournaments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentInvitations",
                table: "TournamentInvitations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentInvitations_AspNetUsers_InvitationSenderUserId",
                table: "TournamentInvitations",
                column: "InvitationSenderUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentInvitations_AspNetUsers_InvitedUserId",
                table: "TournamentInvitations",
                column: "InvitedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentInvitations_Tournaments_TournamentId",
                table: "TournamentInvitations",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentInvitations_AspNetUsers_InvitationSenderUserId",
                table: "TournamentInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentInvitations_AspNetUsers_InvitedUserId",
                table: "TournamentInvitations");

            migrationBuilder.DropForeignKey(
                name: "FK_TournamentInvitations_Tournaments_TournamentId",
                table: "TournamentInvitations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TournamentInvitations",
                table: "TournamentInvitations");

            migrationBuilder.DropColumn(
                name: "TournamentState",
                table: "Tournaments");

            migrationBuilder.RenameTable(
                name: "TournamentInvitations",
                newName: "TournamentInvitation");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentInvitations_TournamentId",
                table: "TournamentInvitation",
                newName: "IX_TournamentInvitation_TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentInvitations_InvitedUserId",
                table: "TournamentInvitation",
                newName: "IX_TournamentInvitation_InvitedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentInvitations_InvitationSenderUserId",
                table: "TournamentInvitation",
                newName: "IX_TournamentInvitation_InvitationSenderUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TournamentInvitation",
                table: "TournamentInvitation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentInvitation_AspNetUsers_InvitationSenderUserId",
                table: "TournamentInvitation",
                column: "InvitationSenderUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentInvitation_AspNetUsers_InvitedUserId",
                table: "TournamentInvitation",
                column: "InvitedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentInvitation_Tournaments_TournamentId",
                table: "TournamentInvitation",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
