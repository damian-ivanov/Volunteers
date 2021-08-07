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
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using static Volunteers.Data.DataConstants;
using Volunteers.Services.Users;
using System.Dynamic;
using Volunteers.Models.Comments;
using Volunteers.Services.Badges;
using Volunteers.Services.Projects;

namespace Volunteers.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IUserService userService;
        private readonly IBadgesService badges;
        private readonly IProjectService projects;


        private readonly VolunteersDbContext data;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ProjectsController(VolunteersDbContext data, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IUserService userService, IBadgesService badges, IProjectService projects)
        {
            this.data = data;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.userService = userService;
            this.badges = badges;
            this.projects = projects;
        }

        public IActionResult Index(string sortOrder)
        {
            ViewData["DateSortParm"] = sortOrder;
            ViewBag.ViewType = sortOrder;

            return View(projects.ListProjects(sortOrder));
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
            var ownerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var extension = "";

            if (image != null)
            {
                extension = Path.GetExtension(image.FileName.ToLower());

                if (!WebConstants.AllowedImageExtensions.Contains(extension))
                {
                    this.ModelState.AddModelError(nameof(project.Image), $"Invalid image format. Allowed images are of type {String.Join(", ",WebConstants.AllowedImageExtensions)}.");
                }
            }

            if (image == null)
            {
                this.ModelState.AddModelError(nameof(project.Image), "Please, upload an image for the project.");
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

            projects.Create(project, projects.AddImage(image, extension), ownerId);

            return RedirectToAction("Confirmation", "Projects");
        }

        public IActionResult Confirmation()
        {
            return View();
        }

        public IActionResult Details(string id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            dynamic mymodel = new ExpandoObject();
            mymodel.project = projects.Details(id, userId);
            mymodel.comment = new AddCommentFormModel();

            return View(mymodel);
        }


        public IActionResult Edit(string id)
        {
            var project = projects.Edit(id);

            project.Categories = this.GetProjectCategories();

            return View(project);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(EditProjectViewModel project, IFormFile image)
        {
            var extension = "";
            var secureImageName = "";

            if (image != null)
            {
                extension = Path.GetExtension(image.FileName.ToLower());

                if (!WebConstants.AllowedImageExtensions.Contains(extension))
                {
                    this.ModelState.AddModelError(nameof(project.Image), $"Invalid image format. Allowed images are of type {String.Join(", ", WebConstants.AllowedImageExtensions)}.");
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
                secureImageName = projects.AddImage(image, extension);
            }
            else
            {
                secureImageName = project.OldImage;
            }

            projects.Edit(project, secureImageName);
            return RedirectToAction("Details", new { id = project.Id });
        }


        [Authorize]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Approve(string id)
        {
            if (!projects.Approve(id))
            {
                return RedirectToAction("Admin", "Projects");
            }

            return Redirect("/Admin/Projects");
        }

        [Authorize]
        public IActionResult Complete(string id) => View(new CompleteProjectFormModel { Id = id });
     
        [Authorize]
        [HttpPost]
        public IActionResult Complete(CompleteProjectFormModel project, IFormFile image)
        {
            var extension = "";

            if (image == null)
            {
                this.ModelState.AddModelError(nameof(project.CompletedImage), "Please, upload an image for the project.");
            }

            if (image != null)
            {
                extension = Path.GetExtension(image.FileName.ToLower());

                if (!WebConstants.AllowedImageExtensions.Contains(extension))
                {
                    this.ModelState.AddModelError(nameof(project.CompletedImage), $"Invalid image format. Allowed images are of type {String.Join(", ", WebConstants.AllowedImageExtensions)}.");
                }
            }

            if (project == null)
            {
                return RedirectToAction("Index", "Projects");
            }

            if (!ModelState.IsValid)
            {
                return View(project);
            }

            var secureImageName = projects.AddImage(image, extension);
            projects.Complete(project, secureImageName);
            
            return RedirectToAction("Details", new { id = project.Id });
        }



        [Authorize]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Activate(string id)
        {
            projects.Activate(id);
            return RedirectToAction("Index", "Projects");
        }

        [Authorize]
        [Authorize(Roles = AdministratorRoleName)]
        public IActionResult Hide(string id)
        {
            projects.Hide(id);
            return RedirectToAction("Index", "Projects", new { area = "Admin" });
        }


        [Authorize]
        public IActionResult Delete(string id)
        {
            if (!User.IsInRole(AdministratorRoleName) && !userService.IsOwner(id, userManager.GetUserId(User)))
            {
                return RedirectToAction("Index", "Projects");
            }

            projects.Delete(id);
            return RedirectToAction("Index", "Projects");
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

