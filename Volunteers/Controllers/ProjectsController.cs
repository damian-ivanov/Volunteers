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

        public IActionResult Create() => View(new AddProjectFormModel
        {
            Categories = this.GetProjectCategories()
        });



        //public IActionResult Add() => View(new AddCarFormModel
        //{
        //    Categories = this.GetCarCategories()
        //});


        [HttpPost]
        public IActionResult Create(AddProjectFormModel project)
        {
            if (!this.data.Categories.Any(c => c.Id == project.CategoryId))
            {
                this.ModelState.AddModelError(nameof(project.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                project.Categories = this.GetProjectCategories();

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


        public IActionResult Edit(string id)
        {
            var project = this.data.Projects.Where(p => p.Id == id).Select(p => new EditProjectViewModel
            {
                Address = p.Address,
                CurrentCategory = p.Category.Name,
                CurrentCategoryId = p.CategoryId,
                City = p.City,
                Description = p.Description,
                StartDate = p.StartDate,
                Title = p.Title
            }).FirstOrDefault();

            project.Categories = this.GetProjectCategories();

            return View(project);
        }


        [HttpPost]
        public IActionResult Edit(EditProjectViewModel project)
        {
            if (!this.data.Categories.Any(c => c.Id == project.CategoryId))
            {
                this.ModelState.AddModelError(nameof(project.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                project.Categories = this.GetProjectCategories();

                return View(project);
            }

            var projectToEdit = this.data.Projects.SingleOrDefault(p => p.Id == project.Id);

            projectToEdit.Address = project.Address;
            projectToEdit.CategoryId = project.CategoryId;
            projectToEdit.City = project.City;
            projectToEdit.Description = project.Description;
            projectToEdit.StartDate = project.StartDate;
            projectToEdit.Title = project.Title;

            data.SaveChanges();

            return RedirectToAction("Index", "Projects");
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


        private IEnumerable<ProjectCategoryViewModel> GetProjectCategories()
        {
            return this.data.Categories.Select(c => new ProjectCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
    }
}

