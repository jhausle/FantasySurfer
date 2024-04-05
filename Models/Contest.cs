using System;

namespace JokesWebApp.Models
{
    public class Contest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public string Country { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
