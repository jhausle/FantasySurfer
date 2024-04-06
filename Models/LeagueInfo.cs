using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace JokesWebApp.Models
{

    public class LeagueInfo
    {
        public LeagueInfo()
        {
            InfoDictionary = new Dictionary<string, SingleLeagueInfo>();
        }

        public int contestID;
        public Dictionary<string, SingleLeagueInfo> InfoDictionary;
    }

    public class SingleLeagueInfo
    {
        public SingleLeagueInfo()
        {
            LeagueName = "Yewww";
            Members = new List<LeagueMember>();
        }

        public string LeagueName;
        public List<LeagueMember> Members;
    }

    public class LeagueMember
    {
        public string TeamName;
        public string OwnerName;
        public string TeamId;
        public decimal Points;
        public int rankingPoints;
        public int rank;
        public WslRoster wslRoster;
        public FsRoster fsRoster;
    }

    public class WslRoster
    {
        public WslRoster()
        {
            tierASurfers = new List<Surfer>();
            tierBSurfers = new List<Surfer>();
            tierCSurfers = new List<Surfer>();
        }
        public List<Surfer> tierASurfers;
        public List<Surfer> tierBSurfers;
        public List<Surfer> tierCSurfers;
    }

    public class FsRoster
    {
        public FsRoster()
        {
            surfers = new List<Surfer>();
        }
        public List<Surfer> surfers;
    }

    public class Surfer
    {
        public string Name;
        public string Country;
        public string Price;
        public decimal Points;
        public bool twoX;
    }
}
