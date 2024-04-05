using JokesWebApp.Data;
using JokesWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace JokesWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // For now we are just going to write all the lines to the db here
            /*var info = new MemberInfo("Bryce", "Miller", "Brycemiller6", "1293240", "bmill666&nbsp;(Admin)", "115705");
            _context.MemberInfo.Add(info);
            info = new MemberInfo("Devon", "Sousa", "Drop-inDev", "1295843", "dsoujaboy", "64989");
                _context.MemberInfo.Add(info);
            info = new MemberInfo("Julian", "Monk", "JMonk92", "1302380", "Jmonk92", "247890");
                _context.MemberInfo.Add(info);
            info = new MemberInfo("John", "Hausle", "jhausle", "1303227", "jhausle", "245816");
                _context.MemberInfo.Add(info);
            info = new MemberInfo("Connor", "Davidge", "29thstreet", "1271756", "29thStreet", "170694");
                _context.MemberInfo.Add(info);
            info = new MemberInfo("Jake", "Miller", "jake_miller", "1302333", "jmiller", "155865");
                _context.MemberInfo.Add(info);
            info = new MemberInfo("Robert", "Parucha", "BobbyP", "1327479", "rcparucha", "224809");
                _context.MemberInfo.Add(info);
            info = new MemberInfo("Nick", "Curci", "curcikid", "1292901", "curcikid", "196595");
                _context.MemberInfo.Add(info);
            info = new MemberInfo("Evan", "Bronner", "Ebronner", "1296568", "ebronzer", "131763");
                _context.MemberInfo.Add(info);
            info = new MemberInfo("Gnarf", "", "gnarf", "1295830", "gnarf310", "232353");
                _context.MemberInfo.Add(info);
            info = new MemberInfo("Dana", "Tuttle", "EGGY_GUY", "1305420", "EGGYGUY", "175725");
                _context.MemberInfo.Add(info);
            info = new MemberInfo("Denton", "VanDuzer", "Dballer69", "1310175", "dballer69", "249507");
            _context.MemberInfo.Add(info);
            info = new MemberInfo("Chaz", "James", "BacksideMcCutty", "1296461", "backsidemccutty", "250375");
            _context.MemberInfo.Add(info);
            info = new MemberInfo("Hunter", "VanDuzer", "MrBuckets", "1315880", "MrBucketsss", "250691");
            _context.MemberInfo.Add(info);
            info = new MemberInfo("Spencer", "Neste", "Nest13", "1311163", "Sneste", "250651");
            _context.MemberInfo.Add(info);
            _context.SaveChanges();
            */


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
