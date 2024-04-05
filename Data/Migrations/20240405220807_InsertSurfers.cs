using Microsoft.EntityFrameworkCore.Migrations;

namespace JokesWebApp.Data.Migrations
{
    public partial class InsertSurfers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(table: "Surfers",
                columns: new[] { "FirstName", "LastName", "FullName", "DateOfBirth", "Country", "Stance" },
                values: new object[,]
                {
                    {"John John", "Florence", "John John Florence", "11/18/1992", "Hawaii", "Regular"},
                    {"Griffin", "Colapinto", "Griffin Colapinto", "07/29/1998", "USA", "Regular"},
                    {"Gabriel", "Medina", "Gabriel Medina", "12/22/1993", "Brazil", "Goofy"},
                    {"Ethan", "Ewing", "Griffin Colapinto", "09/02/1998", "Australia", "Regular"},
                    {"Jack", "Robinson", "Griffin Colapinto", "12/27/1997", "Australia", "Regular"},
                    {"Italo", "Ferreira", "Griffin Colapinto", "05/06/1994", "Brazil", "Goofy"},
                    {"Cole", "Houshmand", "Griffin Colapinto", "12/27/2000", "USA", "Goofy"},
                    {"Kelly", "Slater", "Griffin Colapinto", "12/11/1972", "USA", "Regular"},
                    {"Yago", "Dora", "Griffin Colapinto", "05/18/1996", "Brazil", "Goofy"},
                    {"Samuel", "Pupo", "Griffin Colapinto", "11/18/1992", "Brazil", "Regular"},
                    {"Crosby", "Colapinto", "Griffin Colapinto", "07/15/2001", "USA", "Regular"},
                    {"Kanoa", "Igarashi", "Griffin Colapinto", "10/01/1997", "Japan", "Regular"},
                    {"Baron", "Mamiya", "Griffin Colapinto", "1/27/2000", "Hawaii", "Regular"},
                    {"Caio", "Ibelli", "Griffin Colapinto", "10/11/1993", "Brazil", "Regular"},
                    {"Callum", "Robson", "Griffin Colapinto", "11/27/2000", "Australia", "Regular"},
                    {"Connor", "O'Leary", "Griffin Colapinto", "10/12/1993", "Japan", "Goofy"},
                    {"Deivid", "Silva", "Griffin Colapinto", "02/10/1995", "Brazil", "Goofy"},
                    {"Eli", "Hanneman", "Griffin Colapinto", "11/07/2002", "Hawaii", "Regular"},
                    {"Filipe", "Toledo", "Griffin Colapinto", "04/16/1995", "Brazil", "Regular"},
                    {"Frederico", "Morais", "Griffin Colapinto", "01/03/1992", "Portugal", "Regular"},
                    {"Ian", "Gentil", "Griffin Colapinto", "12/02/1996", "Hawaii", "Regular"},
                    {"Imaikalani", "deVault", "Griffin Colapinto", "11/12/1997", "Hawaii", "Regular"},
                    {"Jacob", "Wilcox", "Griffin Colapinto", "06/02/1997", "Australia", "Goofy"},
                    {"Jake", "Marshall", "Griffin Colapinto", "11/12/1998", "USA", "Regular"},
                    {"Joao", "Chianca", "Griffin Colapinto", "08/30/2000", "Brazil", "Regular"},
                    {"Jordy", "Smith", "Griffin Colapinto", "02/11/1988", "South Africa", "Regular"},
                    {"Kade", "Matson", "Griffin Colapinto", "05/16/2002", "USA", "Regular"},
                    {"Leonardo", "Fioravanti", "Griffin Colapinto", "12/08/1997", "Italy", "Regular"},
                    {"Liam", "O'Brien", "Griffin Colapinto", "05/02/1999", "Australia", "Regular"},
                    {"Matthew", "McGillivray", "Griffin Colapinto", "03/26/1997", "Hawaii", "Regular"},
                    {"Miguel", "Pupo", "Griffin Colapinto", "11/19/1991", "Brazil", "Goofy"},
                    {"Ramzi", "Boukhiam", "Griffin Colapinto", "09/14/1993", "Morocco", "Goofy"},
                    {"Rio", "Waida", "Griffin Colapinto", "01/25/2000", "Indonesia", "Regular"},
                    {"Ryan", "Callinan", "Griffin Colapinto", "05/27/1992", "Australia", "Goofy"},
                    {"Seth", "Moniz", "Griffin Colapinto", "08/09/1997", "Hawaii", "Regular"},
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
