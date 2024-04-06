using Microsoft.EntityFrameworkCore.Migrations;

namespace JokesWebApp.Data.Migrations
{
    public partial class conestupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Contest");

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Contest",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Contest");

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Contest",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
