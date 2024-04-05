using JokesWebApp.Data;
using JokesWebApp.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace JokesWebApp.Adapters
{
    public static class FsAdapter
    {
        public static SingleLeagueInfo LoadLeagueInfo(ApplicationDbContext context, int contestId)
        {
            var wslInfo = new SingleLeagueInfo();
            var memberInfo = context.MemberInfo.ToList();
            foreach (var member in memberInfo)
            {
                var lm = new LeagueMember();
                var points = context.Points
                    .Where(x => x.LeagueId == "1" && x.ContestId == contestId.ToString() && x.TeamId == member.Id)
                    .Select(x => x.Value).FirstOrDefault();
                lm.TeamName = member.FsName;
                lm.Points = points;
                wslInfo.Members.Add(lm);
            }
            return wslInfo;
        }

        public static FsRoster GetRoster(ApplicationDbContext context, int memberId, int contestId)
        {
            var fsInfo = new FsRoster();
            var surfers = new List<int>();
            for (int i = 1; i < 9; i++)
            {
                var temp = context.FsRosters
                    .Where(x => x.ContestId == contestId && x.MemberId == memberId)
                    .Select(x => x.GetType().GetProperty($"Surfer{i}Id").GetValue(x))
                    .FirstOrDefault();
                if (temp != null)
                    surfers.Add((int)temp);
            }
            foreach (var surfer in surfers)
            {
                var temp = new Surfer();
                var dbSurfer = context.Surfers
                    .Where(x => x.Id == surfer)
                    .FirstOrDefault();
                temp.Name = dbSurfer.FullName;
                //temp.Price = // TODO - this will have to be in the roster table I think
                temp.Country = dbSurfer.Country;
                // Get the points for this surfer
                temp.Points = context.SurferPoints
                    .Where(x => x.SurferId == surfer && x.ContestId == contestId && x.LeagueId == 1)
                    .Select(x => x.Points)
                    .FirstOrDefault();
                fsInfo.surfers.Add(temp);
            }
            return fsInfo;
        }
    }
}
