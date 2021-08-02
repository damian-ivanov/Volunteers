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

namespace Volunteers.Controllers
{
    public class HomeController : Controller
    {
        private readonly VolunteersDbContext data;

        public HomeController(VolunteersDbContext data)
        {
            this.data = data;
        }
        public IActionResult Index([FromQuery] AllProjectsQueryModel query)
        {
            ViewBag.ProjectsCount = data.Projects.Where(p => p.IsCompleted == true).Count();
            ViewBag.TownsCount = data.Projects.Where(p => p.IsCompleted == true).Select(c => c.City).Distinct().Count();
            ViewBag.UsersCount = data.Projects.Select(u => u.OwnerId).ToList().Distinct().Count();

            //Search criteria start
            var projectsQuery = this.data.Projects.Where(p => p.IsPublic == true).AsQueryable();
            
            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                projectsQuery = projectsQuery.Where(p => p.City.ToLower() == query.SearchTerm.ToLower());
            }

            if (!string.IsNullOrEmpty(query.Category))
            {
                projectsQuery = projectsQuery.Where(c => c.Category.Name == query.Category);
            }

            if (query.ShowCompleted == false)
            {
                projectsQuery = projectsQuery.Where(p => p.IsCompleted == false);
                //query.ShowCompleted = true;
            }

            var sortOrder = query.SortOrder;

            switch (sortOrder)
            {
                case "Newest":
                    projectsQuery = projectsQuery.OrderByDescending(p => p.PublishedOn);
                    break;
                case "Oldest":
                    projectsQuery = projectsQuery.OrderBy(p => p.PublishedOn);
                    break;
                case "Starting Soon":
                    projectsQuery = projectsQuery.OrderBy(p => p.StartDate);
                    break;
                case "Most participants":
                    projectsQuery = projectsQuery.OrderByDescending(p => p.Users.Count());
                    break;
                default:
                    sortOrder = "Newest";
                    projectsQuery = projectsQuery.OrderByDescending(p => p.PublishedOn);
                    break;
            }
            //Search criteria end

            var categories = this.data.Categories.ToList();

            var totalProjects = projectsQuery.Count();

            var projects = projectsQuery.Skip((query.CurrentPage - 1) * AllProjectsQueryModel.ProjectsPerPage)
                .Take(AllProjectsQueryModel.ProjectsPerPage).Select(p => new ProjectListingViewModel
            {
                Address = p.Address,
                City = p.City,
                Description = p.Description,
                Id = p.Id,
                Participants = p.Users.Count(),
                PublishedOn = p.PublishedOn.ToString("d"),
                StartDate = p.StartDate.ToString("d"),
                Title = p.Title,
                Image = Path.Combine("/uploads/", p.Image),
                IsCompleted = p.IsCompleted,
            }).ToList();

            query.Categories = categories;
            query.Projects = projects;
            query.TotalProjects = totalProjects;

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
