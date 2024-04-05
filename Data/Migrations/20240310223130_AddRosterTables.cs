using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace JokesWebApp.Data.Migrations
{
    public partial class AddRosterTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "Points",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "FsRosters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(nullable: false),
                    ContestId = table.Column<int>(nullable: false),
                    Surfer1Id = table.Column<int>(nullable: false),
                    Surfer2Id = table.Column<int>(nullable: false),
                    Surfer3Id = table.Column<int>(nullable: false),
                    Surfer4Id = table.Column<int>(nullable: false),
                    Surfer5Id = table.Column<int>(nullable: false),
                    Surfer6Id = table.Column<int>(nullable: false),
                    Surfer7Id = table.Column<int>(nullable: false),
                    Surfer8Id = table.Column<int>(nullable: false),
                    SurferAltId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FsRosters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurferPoints",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurferId = table.Column<int>(nullable: false),
                    ContestId = table.Column<int>(nullable: false),
                    LeagueId = table.Column<int>(nullable: false),
                    Points = table.Column<decimal>(nullable: false),
                    Is2x = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurferPoints", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Surfers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Stance = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Surfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WslRosters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(nullable: false),
                    ContestId = table.Column<int>(nullable: false),
                    SurferA1Id = table.Column<int>(nullable: false),
                    SurferA2Id = table.Column<int>(nullable: false),
                    SurferB1Id = table.Column<int>(nullable: false),
                    SurferB2Id = table.Column<int>(nullable: false),
                    SurferB3Id = table.Column<int>(nullable: false),
                    SurferB4Id = table.Column<int>(nullable: false),
                    SurferC1Id = table.Column<int>(nullable: false),
                    SurferC2Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WslRosters", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FsRosters");

            migrationBuilder.DropTable(
                name: "SurferPoints");

            migrationBuilder.DropTable(
                name: "Surfers");

            migrationBuilder.DropTable(
                name: "WslRosters");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "Points");
        }
    }
}
