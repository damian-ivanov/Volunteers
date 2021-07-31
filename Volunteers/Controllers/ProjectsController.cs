﻿using System;
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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using static Volunteers.Data.DataConstants;
using Volunteers.Services.Users;
using System.Dynamic;
using Volunteers.Models.Comments;

namespace Volunteers.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IUserService userService;

        private readonly VolunteersDbContext data;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ProjectsController(VolunteersDbContext data, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IUserService userService)
        {
            this.data = data;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userService = userService;
        }

        public IActionResult Index(string sortOrder)
        {
            var projects = data.Projects.Where(p => p.IsPublic == true);

            //Adding sorting functionality
            ViewData["DateSortParm"] = sortOrder;

            switch (sortOrder)
            {
                case "Newest":
                    projects = projects.OrderByDescending(p => p.PublishedOn);
                    break;
                case "Oldest":
                    projects = projects.OrderBy(p => p.PublishedOn);
                    break;
                case "Starting Soon":
                    projects = projects.OrderBy(p => p.StartDate);
                    break;
                case "Most participants":
                    projects = projects.OrderByDescending(p => p.Users.Count());
                    break;
                default:
                    projects = projects.OrderByDescending(p => p.PublishedOn);
                    break;
            }

            ViewBag.ViewType = sortOrder;

            return View(projects.Select(p => new ProjectListingViewModel
            {
                Address = p.Address,
                City = p.City,
                Description = p.Description,
                Id = p.Id,
                Participants = p.Users.Count(),
                PublishedOn = p.PublishedOn.ToString("d"),
                StartDate = p.StartDate.ToString("d"),
                Title = p.Title,
                IsCompleted = p.IsCompleted,
                Image = Path.Combine("/uploads/", p.Image)
            }).ToList());
        }

        [Authorize]
        public IActionResult Create() => View(new AddProjectFormModel
        {
            Categories = this.GetProjectCategories()
        });


        [HttpPost]
        [Authorize]
        public IActionResult Create(AddProjectFormModel project, IFormFile image)
        {
            var secureImageName = "";
            var extension = "";

            if (image != null)
            {
                extension = Path.GetExtension(image.FileName.ToLower());

                if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                {
                    this.ModelState.AddModelError(nameof(project.Image), "Invalid image format. Allowed images are of type .jpg, .jpeg or .png.");
                }
            }


            if (!this.data.Categories.Any(c => c.Id == project.CategoryId))
            {
                this.ModelState.AddModelError(nameof(project.CategoryId), "Category does not exist.");
            }

            if (project.StartDate < @DateTime.UtcNow)
            {
                this.ModelState.AddModelError(nameof(project.StartDate), "Please, choose a date in the future.");
            }

            if (!ModelState.IsValid)
            {
                project.Categories = this.GetProjectCategories();
                return View(project);
            }

            //Adding image
            string webRootPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            secureImageName = Guid.NewGuid() + extension;

            var folderPath = Path.Combine(webRootPath, "uploads", secureImageName);

            using (var fileStream = new FileStream(folderPath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            //Creating the project
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
                Image = secureImageName,
                OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };


            data.Projects.Add(validProject);
            data.SaveChanges();

            return RedirectToAction("Confirmation", "Projects");
        }

        public IActionResult Confirmation()
        {
            return View();
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
                IsPublic = p.IsPublic,
                Image = Path.Combine("/uploads/", p.Image),
                IsCompleted = p.IsCompleted,
                Comments = p.Comments.Where(c => c.IsPublic).OrderByDescending(c => c.PublishedOn).ToList(),
                OwnerName = this.data.Users.Where(u => u.Id == p.OwnerId).Select(u => u.Email).FirstOrDefault(),
                IsOwner = userService.IsOwner(p.Id, userManager.GetUserId(User)),
                Joined = p.Users.Contains(this.data.Users.Where(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault())
        }).FirstOrDefault();

            dynamic mymodel = new ExpandoObject();
            mymodel.project = project;
            mymodel.comment = new AddCommentFormModel();

            return View(mymodel);
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
                Title = p.Title,
                OldImage = Path.Combine("/uploads/", p.Image),
                Image = Path.Combine("/uploads/", p.Image)
            }).FirstOrDefault();

            project.Categories = this.GetProjectCategories();

            return View(project);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(EditProjectViewModel project, IFormFile image)
        {
            var secureImageName = "";
            var extension = "";

            if (image != null)
            {
                extension = Path.GetExtension(image.FileName.ToLower());

                if (extension != ".jpg" && extension != ".png" && extension != ".jpeg")
                {
                    this.ModelState.AddModelError(nameof(project.Image), "Invalid image format. Allowed images are of type .jpg, .jpeg or .png.");
                }
            }

            if (!this.data.Categories.Any(c => c.Id == project.CategoryId))
            {
                this.ModelState.AddModelError(nameof(project.CategoryId), "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                project.Categories = this.GetProjectCategories();

                return View(project);
            }

            if (image != null)
            {
                //Adding image
                string webRootPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

                secureImageName = Guid.NewGuid() + extension;

                var folderPath = Path.Combine(webRootPath, "uploads", secureImageName);

                using (var fileStream = new FileStream(folderPath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
            }
            else
            {
                secureImageName = project.OldImage;
            }

            var projectToEdit = this.data.Projects.SingleOrDefault(p => p.Id == project.Id);

            projectToEdit.Address = project.Address;
            projectToEdit.CategoryId = project.CategoryId;
            projectToEdit.City = project.City;
            projectToEdit.Description = project.Description;
            projectToEdit.StartDate = project.StartDate;
            projectToEdit.Title = project.Title;
            projectToEdit.Image = secureImageName;
            data.SaveChanges();
            return RedirectToAction("Details", new { id = project.Id });
        }


        [Authorize]
        public IActionResult Approve(string id)
        {
            var project = this.data.Projects.Where(p => p.Id == id).FirstOrDefault();

            if (project == null)
            {
                return RedirectToAction("Admin", "Projects");
            }

            project.IsPublic = true;
            data.SaveChanges();
            return RedirectToAction("Admin", "Projects");
        }

        [Authorize]
        public IActionResult Complete(string id)
        {
            var project = this.data.Projects.Where(p => p.Id == id).FirstOrDefault();

            if (project == null)
            {
                return RedirectToAction("Admin", "Projects");
            }

            project.IsCompleted = true;
            data.SaveChanges();
            return RedirectToAction("Index", "Projects");
        }



        [Authorize]
        public IActionResult Activate (string id)
        {
            var project = this.data.Projects.Where(p => p.Id == id).FirstOrDefault();

            if (project == null)
            {
                return RedirectToAction("Admin", "Projects");
            }

            project.IsCompleted = false;
            data.SaveChanges();
            return RedirectToAction("Index", "Projects");
        }

        [Authorize]
        public IActionResult Hide(string id)
        {
            var project = this.data.Projects.Where(p => p.Id == id).FirstOrDefault();

            if (project == null)
            {
                return RedirectToAction("Admin", "Projects");
            }

            project.IsPublic = false;
            data.SaveChanges();
            return RedirectToAction("Admin", "Projects");
        }


        [Authorize]
        public IActionResult Delete(string id)
        {
            if (!User.IsInRole(AdministratorRoleName) && !userService.IsOwner(id, userManager.GetUserId(User)))
            {
                return RedirectToAction("Index", "Projects");
            }

            var project = this.data.Projects.Where(p => p.Id == id).FirstOrDefault();

            if (project == null)
            {
                return RedirectToAction("Index", "Projects");
            }


            this.data.Projects.Remove(project);
            data.SaveChanges();
            return RedirectToAction("Index", "Projects");
        }

        [Authorize]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Admin()
        {
            ViewBag.ProjectsCount = data.Projects.Where(p => p.IsPublic == false).Count();
            ViewBag.CommentsCount = data.Comments.Where(c => c.IsPublic == false).Count();

            return View(data.Projects.Where(p => p.IsPublic == false).Select(p => new ProjectListingViewModel
            {
                Address = p.Address,
                City = p.City,
                Description = p.Description,
                Id = p.Id,
                Participants = p.Users.Count(),
                PublishedOn = p.PublishedOn.ToString("d"),
                StartDate = p.StartDate.ToString("d"),
                Title = p.Title,
            }).ToList());    
        }

        [Authorize]
        public IActionResult Join(string id)
        {
            var project = this.data.Projects.Include(u => u.Users).FirstOrDefault(p => p.Id == id);
            var user = this.data.Users.Where(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();
            
            if (project == null || user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (project.Users.Contains(user))
            {
                return RedirectToAction("Index", "Home");
            }

            project.Users.Add(user);

            data.SaveChanges();
            return RedirectToAction("Details", new { id = project.Id });
        }

        [Authorize]
        public IActionResult Leave(string id)
        {
            var project = this.data.Projects.Include(u => u.Users).FirstOrDefault(p => p.Id == id);
            var user = this.data.Users.Where(u => u.Id == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefault();

            if (project == null || user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!project.Users.Contains(user))
            {
                return RedirectToAction("Index", "Home");
            }

            project.Users.Remove(user);

            data.SaveChanges();
            return RedirectToAction("Details", new { id = project.Id });
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

