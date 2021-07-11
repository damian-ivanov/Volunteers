using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Volunteers.Data;
using Volunteers.Data.Models;
using Volunteers.Models.Projects;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Volunteers.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly VolunteersDbContext data;

        public ProjectsController(VolunteersDbContext data)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
            return View(data.Projects.Select(p => new ProjectListingViewModel
            {
                Address = p.Address,
                City = p.City,
                Description = p.Description,
                Id = p.Id,
                Participants = p.Users.Count(),
                PublishedOn = p.PublishedOn.ToString("d"),
                StartDate = p.StartDate.ToString("d"),
                Title = p.Title,
                Votes = p.Votes
            }).ToList());
        }

        public IActionResult Create()
        {
            //if (!this.User.Identity.IsAuthenticated)
            //{
            //    return Redirect("/Identity/Account/Login");
            //}

            return View();
        }

        [HttpPost]
        public IActionResult Create(AddProjectFormModel project)
        {
            if (!ModelState.IsValid)
            {
                return View(project);
            }

            var validProject = new Project
            {
                Address = project.Address,
                CategoryId = project.CategoryId,
                City = project.City,
                Description = project.Description,
                IsPublic = false,
                PublishedOn = DateTime.Now,
                StartDate = project.StartDate,
                Title = project.Title,
                OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

           

            data.Projects.Add(validProject);
            data.SaveChanges();

            return RedirectToAction("Index", "Projects");
        }

        public IActionResult Details(string id)
        {

            var project = this.data.Projects.Where(p => p.Id == id).Select(p => new ProjectDetailViewModel
            {
                Address = p.Address,
                Category = p.Category.Name,
                City = p.City,
                Description = p.Description,
                PublishedOn = p.PublishedOn.ToString("d"),
                StartDate = p.StartDate.ToString("d"),
                Title = p.Title,
                Id = p.Id,
                Participants = p.Users.Count(),
                Votes = p.Votes,
               // Owner = this.data.Users.Where(u => u.Id == p.OwnerId).Select(u => u.Email).FirstOrDefault()
            }).FirstOrDefault();

            return View(project);
        }

        public IActionResult Delete(string id)
        {
            var project = this.data.Projects.Where(p => p.Id == id).FirstOrDefault();

            if (project == null)
            {
                return RedirectToAction("Index", "Home");
            }

            this.data.Projects.Remove(project);
            data.SaveChanges();
            return RedirectToAction("Index", "Projects");
        }

    }
}

