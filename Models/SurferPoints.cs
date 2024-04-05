namespace JokesWebApp.Models
{
    public class SurferPoints
    {
        public int Id { get; set; }
        public int SurferId { get; set; }
        public int ContestId { get; set; }
        public int LeagueId { get; set; }
        public decimal Points { get; set; }
        public bool Is2x { get; set; }
    }
}
