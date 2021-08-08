using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volunteers.Data;
using Volunteers.Models;
using Volunteers.Models.Projects;
using Volunteers.Services.Projects;
using Volunteers.Services.Stats;

namespace Volunteers.Controllers
{
    public class HomeController : Controller
    {
 
        private readonly IStatsService stats;
        private readonly IProjectService projects;

        public HomeController(IStatsService stats, IProjectService projects)
        {
            this.stats = stats;
            this.projects = projects;
        }
        public IActionResult Index([FromQuery] AllProjectsQueryModel query)
        {
            ViewBag.ProjectsCount = stats.GetHomePageStats().Item1;
            ViewBag.TownsCount = stats.GetHomePageStats().Item2;
            ViewBag.UsersCount = stats.GetHomePageStats().Item3;

            projects.ListProjectsHomePage(query);

            return View(query);
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
