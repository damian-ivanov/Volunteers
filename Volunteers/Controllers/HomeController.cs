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
            ViewBag.ProjectsCount = data.Projects.Where(p => p.IsCompleted == true).Count();
            ViewBag.TownsCount = data.Projects.Select(c => c.City).Count();
            ViewBag.UsersCount = data.Projects.Select(u => u.OwnerId).ToList().Distinct().Count();

            return View(data.Projects.OrderByDescending(p => p.PublishedOn).Take(3).Select(p => new ProjectListingViewModel
            {
                Address = p.Address,
                City = p.City,
                Description = p.Description,
                Id = p.Id,
                Participants = p.Users.Count(),
                PublishedOn = p.PublishedOn.ToString("d"),
                StartDate = p.StartDate.ToString("d"),
                Title = p.Title,
                Votes = p.Votes,
                IsCompleted = p.IsCompleted,
            }).ToList());
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
