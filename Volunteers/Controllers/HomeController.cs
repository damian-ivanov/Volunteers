using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
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

        public IActionResult Error()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
