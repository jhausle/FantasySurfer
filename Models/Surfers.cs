using System;

namespace JokesWebApp.Models
{
    public class Surfers
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
        public string Stance { get; set; }
    }
}
