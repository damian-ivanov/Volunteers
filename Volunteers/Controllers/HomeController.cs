using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Volunteers.Data;
using Volunteers.Models;
using Volunteers.Models.Projects;

namespace Volunteers.Controllers
{
    public class HomeController : Controller
    {
        private readonly VolunteersDbContext data;

        public HomeController(VolunteersDbContext data)
        {
            this.data = data;
        }
        public IActionResult Index()
        {
            return View(new ProjectDashboardModel
            {
                ParticipantsCount = data.Users.Local.Count(),
                ProjectsCount = data.Projects.Count(),
                TownsCount = data.Projects.Select(c => c.City).Count()
            });
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
