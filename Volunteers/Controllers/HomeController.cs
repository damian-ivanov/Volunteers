using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Volunteers.Models;
using Volunteers.Models.Projects;
using Volunteers.Services.Notifications;
using Volunteers.Services.Projects;
using Volunteers.Services.Stats;

namespace Volunteers.Controllers
{
    public class HomeController : BaseController
    {
 
        private readonly IStatsService stats;
        private readonly IProjectService projects;

        public HomeController(INotificationsService notifications, IStatsService stats, IProjectService projects) : base(notifications)
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

        public IActionResult Notifications()
        {
            return View();
        }
    }
}
