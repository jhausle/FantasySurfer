using JokesWebApp.Data;
using System.Collections.Generic;
using System.Linq;

namespace JokesWebApp.Repositories
{
    public static class PointsRespository
    {

        public static decimal GetPointsByContestAndTeam(ApplicationDbContext context, string contestId, int teamId, string leagueId)
        {
            return context.Points
                .Where(x => x.ContestId == contestId && x.TeamId == teamId.ToString() && x.LeagueId == leagueId)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public static Dictionary<int, decimal> GetPointsByContest(ApplicationDbContext context, string contestId, string leagueId)
        {
            var pointsList = context.Points
                .Where(x => x.ContestId == contestId && x.LeagueId == leagueId)
                .Select(x => new { x.TeamId, x.Value })
                .ToList();

            return pointsList.ToDictionary(p => int.Parse(p.TeamId), p => p.Value);
        }
    }
}
