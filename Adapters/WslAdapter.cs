using JokesWebApp.Data;
using JokesWebApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace JokesWebApp.Adapters
{
    public static class WslAdapter
    {
        public static SingleLeagueInfo LoadLeagueInfo(ApplicationDbContext context, int contestId)
        {
            var wslInfo = new SingleLeagueInfo();
            var memberInfo = context.MemberInfo.ToList();
            foreach (var member in memberInfo)
            {
                var lm = new LeagueMember();
                var points = context.Points
                    .Where(x => x.LeagueId == "WSL" && x.ContestId == contestId.ToString() && x.TeamId == member.Id.ToString())
                    .Select(x => x.Value).FirstOrDefault();
                lm.TeamName = member.WslName;
                lm.Points = points;
                lm.OwnerName = member.FirstName + " " + member.LastName;
                wslInfo.Members.Add(lm);
            }
            wslInfo.Members = wslInfo.Members.OrderByDescending(mem => mem.Points).ToList();

            // now that its ordered, we need to compute ranking
            int rank = 0;
            decimal prevPoints = 0;
            int numTied = 1;
            foreach(var member in wslInfo.Members)
            {
                if (member.Points != prevPoints)
                {
                    rank += numTied;
                    numTied = 1;
                } else
                {
                    numTied++;
                }
                member.rank = rank;
                prevPoints = member.Points;
            }
            return wslInfo;
        }
        public static WslRoster GetRoster(ApplicationDbContext context, int memberId, int contestId)
        {
            var wslInfo = new WslRoster();
            var surfers = new List<int>();
            var twoxIndx = context.WslRosters
                    .Where(x => x.ContestId == contestId && x.MemberId == memberId)
                    .Select(x=> x.twoXSurferIndex)
                    .FirstOrDefault();
            for (int i = 1; i < 3; i++)
            {
                var temp = context.FsRosters
                    .Where(x => x.ContestId == contestId && x.MemberId == memberId)
                    .Select(x => x.GetType().GetProperty($"SurferA{i}Id").GetValue(x))
                    .FirstOrDefault();
                if (temp != null)
                    surfers.Add((int)temp);
            }
            for (int i = 1; i < 5; i++)
            {
                var temp = context.FsRosters
                    .Where(x => x.ContestId == contestId && x.MemberId == memberId)
                    .Select(x => x.GetType().GetProperty($"SurferB{i}Id").GetValue(x))
                    .FirstOrDefault();
                if (temp != null)
                    surfers.Add((int)temp);
            }
            for (int i = 1; i < 3; i++)
            {
                var temp = context.FsRosters
                    .Where(x => x.ContestId == contestId && x.MemberId == memberId)
                    .Select(x => x.GetType().GetProperty($"SurferC{i}Id").GetValue(x))
                    .FirstOrDefault();
                if (temp != null)
                    surfers.Add((int)temp);
            }
            int j = 1;
            foreach (var surfer in surfers)
            {
                var temp = new Surfer();
                var dbSurfer = context.Surfers
                    .Where(x => x.Id == surfer)
                    .FirstOrDefault();
                temp.Name = dbSurfer.FullName;
                //temp.Price = // TODO - this will have to be in the roster table I think
                if (j - 1 == twoxIndx)
                    temp.twoX = true;
                temp.Country = dbSurfer.Country;
                // Get the points for this surfer
                temp.Points = context.SurferPoints
                    .Where(x => x.SurferId == surfer && x.ContestId == contestId && x.LeagueId == 1)
                    .Select(x => x.Points)
                    .FirstOrDefault();
                if (j < 3)
                {
                    wslInfo.tierASurfers.Add(temp);
                } else if (j < 7)
                {
                    wslInfo.tierBSurfers.Add(temp);
                } else
                {
                    wslInfo.tierCSurfers.Add(temp);
                }
                j++;
            }
            return wslInfo;
        }
    }
}
