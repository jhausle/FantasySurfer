using System;

namespace JokesWebApp.Models
{
    public class Points
    {
        public string Id { get; set; }
        public string ContestId { get; set; }
        public int TeamId { get; set; } // this is the member info ID
        public string LeagueId { get; set; }
        public decimal Value { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
