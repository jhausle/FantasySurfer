using HtmlAgilityPack;
using JokesWebApp.Adapters;
using JokesWebApp.Data;
using JokesWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JokesWebApp.Controllers
{
    
    public class FantasyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private List<string> _fans = new List<string>();
        private SingleLeagueInfo Info = new SingleLeagueInfo();
        private SingleLeagueInfo FSInfo = new SingleLeagueInfo();
        private LeagueInfo TotalInfo = new LeagueInfo();
        private readonly string fsurl = "https://fantasy.surfer.com/club/members/?id=6956";
        private readonly int ContestId = 3;
        public FantasyController(ApplicationDbContext context) {
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            /*            var lastupdated = _context.Contest
                            .Where(x => x.Id == ContestId)
                            .Select(x=> x.LastUpdatedAt).FirstOrDefault();
                        var goToDb = (lastupdated < DateTime.Now.AddMinutes(5));
            */
            var goToDb = false;
            if (goToDb)
            {
                // Get WSL league standings
                Info = WslAdapter.LoadLeagueInfo(_context, ContestId);

                // Get FS league standing
                FSInfo = FsAdapter.LoadLeagueInfo(_context, ContestId);
            }
            else
            {
                string url = "https://ctfantasy.worldsurfleague.com/group/165417";
                var resp = CallUrl(url).Result;
                Info = ParseWSLLeagueInfo(resp);
                TotalInfo.InfoDictionary.Add("WSL", Info);

                // Get FS league standings
                var resp2 = CallUrl(fsurl).Result;
                FSInfo = await ParseFSLeagueInfoAsync(resp2);
                TotalInfo.InfoDictionary.Add("FS", FSInfo);

                // Update the DB that we recently web scraped
                var contestEntity = _context.Contest.Find(3);
                contestEntity.LastUpdatedAt = DateTime.Now;
                _context.SaveChanges();
            }

            // Take both and combine into one ranking
            var totalInfo = CombineRankings(Info, FSInfo);
            TotalInfo.InfoDictionary.Add("Total", totalInfo);

            return View(TotalInfo);
        }

        private SingleLeagueInfo CombineRankings(SingleLeagueInfo wslInfo, SingleLeagueInfo fsInfo)
        {
            var totalInfo = new SingleLeagueInfo();
            var members = _context.MemberInfo.Select(x => x).ToList();
            foreach(var member in members)
            {
                var temp = new LeagueMember();
                var wslTeamName = member.WslName;
                var fsTeamName = member.FsName;
                temp.TeamName = member.FirstName + " " + member.LastName;
                temp.rankingPoints = wslInfo.Members.Where(x => x.TeamName == wslTeamName).FirstOrDefault().rank
                    + fsInfo.Members.Where(x => x.TeamName == fsTeamName).FirstOrDefault().rank;
                temp.Points = wslInfo.Members.Where(x => x.TeamName == wslTeamName).FirstOrDefault().Points
                    + fsInfo.Members.Where(x => x.TeamName == fsTeamName).FirstOrDefault().Points;
                totalInfo.Members.Add(temp);
            }
            totalInfo.Members = totalInfo.Members.OrderByDescending(mem => mem.Points).ToList();
            // now that its ordered, we need to compute ranking
            int rank = 0;
            decimal prevPoints = 0;
            int numTied = 1;
            foreach (var member in totalInfo.Members)
            {
                if (member.Points != prevPoints)
                {
                    rank += numTied;
                    numTied = 1;
                }
                else
                {
                    numTied++;
                }
                member.rank = rank;
                prevPoints = member.Points;
            }
            return totalInfo;
        }

        private static async Task<string> CallUrl(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(url);
            return response;
        }

        // This method gets the FS league standings
        private async Task<SingleLeagueInfo> ParseFSLeagueInfoAsync(string html)
        {
            var info = new SingleLeagueInfo();
            // First need to login
            using (HttpClient client = new HttpClient())
            {
                var formData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("password", "8c03a203fa67e8cfb35c9ff32b5f46cb09c90562"),
                    new KeyValuePair<string, string>("username", "jthausle11@yahoo.com"),
                    new KeyValuePair<string, string>("legacy_password", "Mbenga28"),
                    new KeyValuePair<string, string>("persistent", "on"),
                    new KeyValuePair<string, string>("submit", "Login")
                });
                string url = "https://fantasy.surfer.com/login/";
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Referer = url;
                request.CookieContainer= new CookieContainer();
                string postData = "password=8c03a203fa67e8cfb35c9ff32b5f46cb09c90562&username=jthausle11%40yahoo.com&legacy_password=Mbenga28&persistent=on&submit=Login";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentLength= byteArray.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                }
                //HttpResponseMessage response = await client.PostAsync(url, formData);

                using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
                {
                    if (resp.StatusCode == HttpStatusCode.OK)
                    {
                        var sessionCookie = resp.Cookies[0].Value;

                        Console.WriteLine(sessionCookie);

                        client.DefaultRequestHeaders.Add("Cookie", $"PHPSESSID={sessionCookie}");
                        var clubContentResponse = await client.GetAsync(fsurl);
                        if (clubContentResponse.IsSuccessStatusCode)
                        {
                            string clubContent = await clubContentResponse.Content.ReadAsStringAsync();
                            info = ParseFSLeagueContent(clubContent);
                        }
                    }
                }
            }
            return info;
        }

        private SingleLeagueInfo ParseFSLeagueContent(string content)
        {


            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(content);
            var leagueInfo = new SingleLeagueInfo();

            var teamInfo = htmlDoc.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "").Contains("table-row table-bg")).ToList();

            foreach (var team in teamInfo)
            {
                var tempMember = new LeagueMember();
                // Grab the team name
                var teamName = team.Descendants("span").FirstOrDefault();
                var temp = teamName.InnerText.Trim();
                tempMember.TeamName = temp;

                // Grab the latest event total
                var eventTotals = team.Descendants("div")
                    .Where(node => node.GetAttributeValue("class", "").Contains("memhead-digits")).FirstOrDefault();
                // Grab last even total from list
                var total = eventTotals.Descendants("span").LastOrDefault().InnerText.Trim();
                tempMember.Points = decimal.Parse(total);

                // Grab the team ID while we are here
                // href = "/team/mens/?user=TEAMID"
                var teamId = team.Descendants("a")
                    .Where(node => node.GetAttributeValue("href", "").Contains("team/mens")).FirstOrDefault();
                var href = teamId.Attributes[0].Value;
                var id = href.Substring(href.IndexOf("=")+1);
                tempMember.TeamId = id;
                
                // get owner name from db
                var firstName = _context.MemberInfo.Where(x => x.FsName == tempMember.TeamName).Select(x => x.FirstName).FirstOrDefault();
                var lastName = _context.MemberInfo.Where(x => x.FsName == tempMember.TeamName).Select(x => x.LastName).FirstOrDefault();
                tempMember.OwnerName = firstName + " " + lastName;
                leagueInfo.Members.Add(tempMember);
            }
            leagueInfo.Members = leagueInfo.Members.OrderByDescending(mem => mem.Points).ToList();

            // now that its ordered, we need to compute ranking
            int rank = 0;
            decimal prevPoints = 0;
            int numTied = 1;
            foreach(var member in leagueInfo.Members)
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

            

            return leagueInfo;
        }

        private string GetPhpSessionId(HttpResponseMessage response)
        {
            if (response.Headers.TryGetValues("Set-Cookie", out var setCookieValues))
            {
                foreach (var cookie in setCookieValues)
                {
                    if(cookie.StartsWith("PHPSESSID"))
                    {
                        return cookie.Split(';')[0].Substring("PHPSESSID=".Length);
                    }
                }
            }
            return "po5er59o21ctcv6okbaue87391";
        }

        // This method gets all the WSL info
        private SingleLeagueInfo ParseWSLLeagueInfo(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var leagueInfo = new SingleLeagueInfo();

            var leagueName = htmlDoc.DocumentNode.Descendants("h1")
                .Where(node => node.GetAttributeValue("class", "") == "page__title")
                .FirstOrDefault()
                ?.InnerText.Trim();
           leagueInfo.LeagueName = leagueName;

            var TeamInfo = htmlDoc.DocumentNode.Descendants("tr")
                .Where(node => node.GetAttributeValue("class", "").Contains("groupTeam"));

            foreach (var team in TeamInfo)
            {
                var tempMember = new LeagueMember();
                // Grab the team ID
                var teamId = team.GetAttributeValue("class", "");
                var trimmed = teamId.Substring(0, teamId.IndexOf(' '));
                tempMember.TeamId = teamId;

                // Grab the team Name
                var teamName = team.Descendants("div")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "") == "user-name")
                    ?.InnerText.Trim();
                tempMember.TeamName = teamName;


                // Grab the point total
                var teamPoints = team.Descendants("td")
                    .FirstOrDefault(node => node.GetAttributeValue("class", "").Contains("groupTeamPts"))
                    ?.InnerText.Trim();
                tempMember.Points = decimal.Parse(teamPoints);
                // get owner name from db
                var firstName = _context.MemberInfo.Where(x => x.WslName == tempMember.TeamName).Select(x => x.FirstName).FirstOrDefault();
                var lastName = _context.MemberInfo.Where(x => x.WslName == tempMember.TeamName).Select(x => x.LastName).FirstOrDefault();
                tempMember.OwnerName = firstName + " " + lastName;
                leagueInfo.Members.Add(tempMember);
            }

            // This comes pre sorted but lets sort it anyway
            leagueInfo.Members = leagueInfo.Members.OrderByDescending(mem => mem.Points).ToList();

            // now that its ordered, we need to compute ranking
            int rank = 0;
            decimal prevPoints = -1;
            int numTied = 1;
            foreach (var member in leagueInfo.Members)
            {
                if (member.Points != prevPoints)
                {
                    rank += numTied;
                    numTied = 1;
                }
                else
                {
                    numTied++;
                }
                member.rank = rank;
                prevPoints = member.Points;
            }
            return leagueInfo;
        }

            private List<string> ParseHtml(string html)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var programmerLinks = htmlDoc.DocumentNode.Descendants("li")
                .Where(node => !node.GetAttributeValue("class", "").Contains("tocsection") && string.IsNullOrEmpty(node.GetAttributeValue("id", "")))
                .ToList();

            List<string> wikiLink = new List<string>();

            foreach(var link in programmerLinks)
            {
                if(link.FirstChild.Attributes.Count > 0)
                {
                    if (!link.FirstChild.Attributes[0].Value.Contains("/wiki/"))
                    { continue; }
                    wikiLink.Add(link.FirstChild.Attributes[1].Value);
                }
            }
            return wikiLink;
        }
    }
}
