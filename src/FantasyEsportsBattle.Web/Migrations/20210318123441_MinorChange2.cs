using Microsoft.EntityFrameworkCore.Migrations;

namespace FantasyEsportsBattle.Web.Migrations
{
    public partial class MinorChange2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "StartingCurrency",
                table: "Tournaments",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartingCurrency",
                table: "Tournaments");
        }
    }
}
