using HtmlAgilityPack;
using JokesWebApp.Adapters;
using JokesWebApp.Data;
using JokesWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace JokesWebApp.Controllers
{
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Get all the team names first
            var leagueInfo = GetTeams().Result;

            return View(leagueInfo);
        }

        private async Task<LeagueInfo> GetTeams()
        {
            var members = _context.MemberInfo.ToList();
            SingleLeagueInfo FsInfo = new SingleLeagueInfo();
            SingleLeagueInfo WsInfo = new SingleLeagueInfo();
            bool goToDb = false;
            int contestId = 3;

            foreach (var member in members)
            {
                if (goToDb)
                {
                    var mem = new LeagueMember();
                    mem.OwnerName = member.FirstName + " " + member.LastName;
                    mem.TeamName = member.FsName;
                    mem.fsRoster = FsAdapter.GetRoster(_context, member.Id, contestId);
                    FsInfo.Members.Add(mem);
                    var mem2 = new LeagueMember();
                    mem2.OwnerName = mem.OwnerName;
                    mem2.TeamName = member.WslName;
                    mem.wslRoster = WslAdapter.GetRoster(_context, member.Id, contestId);
                    WsInfo.Members.Add(mem2);
                }
                else
                {
                    var mem = new LeagueMember();
                    mem.OwnerName = member.FirstName + " " + member.LastName;
                    mem.TeamName = member.FsName;
                    mem.fsRoster = await DownloadFsTeamAsync(member.FsId);
                    FsInfo.Members.Add(mem);
                    var mem2 = new LeagueMember();
                    mem2.OwnerName = mem.OwnerName;
                    mem2.TeamName = member.WslName;
                    mem2.wslRoster = DownloadWslTeam(member.WslId);
                    WsInfo.Members.Add(mem2);
                }
            }
            var LeagueInfo= new LeagueInfo();
            LeagueInfo.InfoDictionary.Add("WSL", WsInfo);
            LeagueInfo.InfoDictionary.Add("FS", FsInfo);
            return LeagueInfo;
        }

        private async Task<FsRoster> DownloadFsTeamAsync(string teamId)
        {
            string baseUrl = "https://fantasy.surfer.com/team/mens/?user=";

            string totalUrl = baseUrl + teamId;
            var roster = new FsRoster();

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Cookie", "PHPSESSID=03rtbk0gt0rf6i35fk1a2rimll");
            var response = await client.GetStringAsync(totalUrl);
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);

            var dash = htmlDoc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "") == "history-row")
                .LastOrDefault();

            //get the surfers name
            var surfers = dash.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "") == ("history-surfer"))
                .ToList();
            // List of the <li> tags that have all the surfers
            foreach(var surfer in surfers)
            {
                var temp = new Surfer();
                var info = surfer.Descendants("span").FirstOrDefault();
                if (info.InnerText.StartsWith("Surfer"))
                    continue;
                List<string> surferInfo = new List<string>();
                var parts = info.InnerText.Split(" ");

                temp.Name = parts[0] + " " + parts[1];
                //temp.Price = surferInfo[2];
                roster.surfers.Add(temp);
            }

            // get the surfers points
            var points = dash.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "") == ("history-price"))
                .ToList();
            // List of the <li> tags that have all the surfers
            int i = 0;
            foreach (var surfer in points)
            {
                var info = surfer.Descendants("span").FirstOrDefault();
                string pointStr = info.InnerText;
                if (pointStr.StartsWith("Points"))
                    continue;

                if (pointStr == "-")
                    pointStr = "0";

                roster.surfers[i++].Points = decimal.Parse(pointStr);
            }

            // get the surfers prices
            var prices = dash.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "") == ("history-points"))
                .ToList();
            // List of the <li> tags that have all the surfers
            i = 0;
            foreach (var surfer in prices)
            {
                var info = surfer.Descendants("span").FirstOrDefault();
                if (info.InnerText.StartsWith("Cost"))
                    continue;

                roster.surfers[i++].Price = info.InnerText;
            }

            return roster;
        }


            private WslRoster DownloadWslTeam(string teamId)
        {
            string baseUrl = "https://ctfantasy.worldsurfleague.com/team/";
            string endUrl = "/roster?displayType=gameStop&gameStopNumber=2";
            //TODO - temp me only
            var myteam = "1303227";
            string totalUrl = baseUrl + myteam + endUrl;
            var roster = new WslRoster();

            var html = CallUrl(totalUrl).Result;
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var rosterTableList = htmlDoc.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("class", "").Contains("tableType-teamRosterSlot")).ToList();
            var mensArea = htmlDoc.DocumentNode.Descendants("table")
                .Where(node => node.GetAttributeValue("class", "").Contains("tableType-teamRosterSlot"))
                .LastOrDefault();
            // now have gotten to men's table. Need to get Tier A Surfers

            for (int i = 0; i < 2; i++)
            {
                var first= new Surfer();
                var rowString = $"teamRosterSlot-A{i + 1}";
                var a1slot = mensArea.Descendants("tr")
                    .Where(node => node.GetAttributeValue("class", "").Contains(rowString))
                    .LastOrDefault();
                var athlete = a1slot.Descendants("span");
                var name = athlete.Where(node => node.GetAttributeValue("class", "") == "athlete-name")
                    .FirstOrDefault().InnerText;
                var country = athlete.Where(node => node.GetAttributeValue("class", "") == "athlete-country-name")
                    .FirstOrDefault().InnerText;
                var pointsTd = a1slot.Descendants("td")
                    .Where(node => node.GetAttributeValue("class", "").StartsWith("teamAthleteFantasyEventPts"))
                    .FirstOrDefault();
                var points = pointsTd.Descendants("span").LastOrDefault().InnerText;
                first.Points = decimal.Parse(points);
                first.Name = name;
                first.Country = country;
                roster.tierASurfers.Add(first);
            }
            for (int i=0; i<4; i++)
            {
                var first= new Surfer();
                var rowString = $"teamRosterSlot-B{i + 1}";
                var a1slot = mensArea.Descendants("tr")
                    .Where(node => node.GetAttributeValue("class", "").Contains(rowString))
                    .LastOrDefault();
                var athlete = a1slot.Descendants("span");
                var name = athlete.Where(node => node.GetAttributeValue("class", "") == "athlete-name")
                    .FirstOrDefault().InnerText;
                var country = athlete.Where(node => node.GetAttributeValue("class", "") == "athlete-country-name")
                    .FirstOrDefault().InnerText;
                var pointsTd = a1slot.Descendants("td")
                    .Where(node => node.GetAttributeValue("class", "").StartsWith("teamAthleteFantasyEventPts"))
                    .FirstOrDefault();
                var points = pointsTd.Descendants("span").FirstOrDefault().InnerText;
                first.Points = decimal.Parse(points);
                first.Name = name;
                first.Country = country;
                roster.tierBSurfers.Add(first);
            }
            for (int i=0; i<2; i++)
            {
                var first= new Surfer();
                var rowString = $"teamRosterSlot-C{i + 1}";
                var a1slot = mensArea.Descendants("tr")
                    .Where(node => node.GetAttributeValue("class", "").Contains(rowString))
                    .LastOrDefault();
                var athlete = a1slot.Descendants("span");
                var name = athlete.Where(node => node.GetAttributeValue("class", "") == "athlete-name")
                    .FirstOrDefault().InnerText;
                var country = athlete.Where(node => node.GetAttributeValue("class", "") == "athlete-country-name")
                    .FirstOrDefault().InnerText;
                var pointsTd = a1slot.Descendants("td")
                    .Where(node => node.GetAttributeValue("class", "").StartsWith("teamAthleteFantasyEventPts"))
                    .FirstOrDefault();
                var points = pointsTd.Descendants("span").FirstOrDefault().InnerText;
                first.Points = decimal.Parse(points);
                first.Name = name;
                first.Country = country;
                roster.tierCSurfers.Add(first);
            }
            return roster;
        }

        private static async Task<string> CallUrl(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(url);
            return response;
        }
    }
}
