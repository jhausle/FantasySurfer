using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace JokesWebApp.Data.Migrations
{
    public partial class UpdateContest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Contest",
                nullable: false,
                defaultValue: false
                );
            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Contest",
                nullable: false,
                defaultValue: false
                );
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedAt",
                table: "Contest",
                nullable: true
                );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
