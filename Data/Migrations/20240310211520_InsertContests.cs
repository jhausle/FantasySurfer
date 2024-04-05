using Microsoft.EntityFrameworkCore.Migrations;
using System.Threading.Tasks.Dataflow;

namespace JokesWebApp.Data.Migrations
{
    public partial class InsertContests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "Contest",
                columns: new[] { "Name", "Description", "Year", "Country" },
                values: new object[,]
                {
                    {"Lexus Pipe Pro", "Pipeline", "2024", "USA"},
                    {"Hurley Pro Sunset Beach", "Sunset", "2024", "USA"},
                    { "MEO Rip Curl Pro Portugal Presented By Corona", "", "2024", "Portugal"},
                    { "Rip Curl Pro Bells Beach", "Bells", "2024", "Australia"},
                    { "Western Australia Margaret River Pro", "Margs", "2024", "Australia"},
                    { "SHISEIDO Tahiti Pro", "Chopes", "2024", "Tahiti"},
                    { "Surf City El Salvador Pro Presented By Corona", "El Sal", "2024", "El Salvador"},
                    { "VIVO Rio Pro Presented By Corona", "Rio", "2024", "Brazil"},
                    { "Corona Fiji Pro", "Fiji", "2024", "Fiji"},
                    { "Rip Curl WSL Finals", "Finals", "2024", "USA"}
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
