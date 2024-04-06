using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JokesWebApp.Models
{
    public class Points
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string ContestId { get; set; }
        public string TeamId { get; set; } // this is the member info ID
        public string LeagueId { get; set; }
        public decimal Value { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
