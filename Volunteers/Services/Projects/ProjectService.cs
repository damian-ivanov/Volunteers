using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Volunteers.Data;
using Volunteers.Data.Models;
using Volunteers.Models.Projects;
using Volunteers.Services.Badges;
using Volunteers.Services.Projects.Models;
using Volunteers.Services.Users;

namespace Volunteers.Services.Projects

{
    public class ProjectService : IProjectService
    {
        private readonly VolunteersDbContext data;
        private readonly IUserService userService;
        private readonly IBadgesService badges;

        public ProjectService(VolunteersDbContext data, IUserService userService, IBadgesService badges)
        {
            this.data = data;
            this.userService = userService;
            this.badges = badges;
        }


        public IEnumerable<ProjectListingServiceModel> ListProjects(string sortOrder)
        {
            var projects = data.Projects.Where(p => p.IsPublic == true);

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


            return projects.Select(p => new ProjectListingServiceModel
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
                Comments = p.Comments.Count(),
                Image = Path.Combine("/uploads/", p.Image)
            }).ToList();
        }


        public void Create(AddProjectFormModel project, string secureImageName, string ownerId)
        {
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
                CompletedImage = "",
                OwnerId = ownerId
            };

            data.Projects.Add(validProject);
            data.SaveChanges();
        }

        public string AddImage(IFormFile image, string extension)
        {
            string webRootPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var secureImageName = Guid.NewGuid() + extension;

            var folderPath = Path.Combine(webRootPath, "uploads", secureImageName);

            using (var fileStream = new FileStream(folderPath, FileMode.Create))
            {
                image.CopyTo(fileStream);
            }

            return secureImageName;
        }

        public ProjectDetailsServiceModel Details(string id, string userId)
        {
            return this.data.Projects.Where(p => p.Id == id).Select(p => new ProjectDetailsServiceModel
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
                CompletedImage = Path.Combine("/uploads/", p.CompletedImage),
                Comments = p.Comments.Where(c => c.IsPublic).OrderByDescending(c => c.PublishedOn).ToList(),
                OwnerName = this.data.Users.Where(u => u.Id == p.OwnerId).Select(u => u.UserName).FirstOrDefault(),
                IsOwner = userService.IsOwner(p.Id, userId),
                Joined = p.Users.Contains(this.data.Users.Where(u => u.Id == userId).FirstOrDefault())
            }).FirstOrDefault();
        }

        public EditProjectViewModel Edit(string id)
        {
            return this.data.Projects.Where(p => p.Id == id).Select(p => new EditProjectViewModel
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
        }

        public void Edit(EditProjectViewModel project, string secureImageName)
        {
            var projectToEdit = this.data.Projects.SingleOrDefault(p => p.Id == project.Id);

            projectToEdit.Address = project.Address;
            projectToEdit.CategoryId = project.CategoryId;
            projectToEdit.City = project.City;
            projectToEdit.Description = project.Description;
            projectToEdit.StartDate = project.StartDate;
            projectToEdit.Title = project.Title;
            projectToEdit.Image = secureImageName;
            data.SaveChanges();
        }

        public bool Approve(string id)
        {
            var project = this.data.Projects.Where(p => p.Id == id).FirstOrDefault();

            if (project == null)
            {
                return false;
            }

            project.IsPublic = true;
            data.SaveChanges();

            badges.Evaluate(project.OwnerId);
            return true;
        }

        public void Complete(CompleteProjectFormModel project, string secureImageName)
        {
            var completedProject = this.data.Projects.Where(p => p.Id == project.Id).FirstOrDefault();
            completedProject.IsCompleted = true;
            completedProject.CompletedImage = secureImageName;
            data.SaveChanges();
        }

        public void Activate(string id)
        {
            var project = this.data.Projects.Where(p => p.Id == id).FirstOrDefault();

            project.IsCompleted = false;
            data.SaveChanges();
        }

        public void Hide(string id)
        {
            var project = this.data.Projects.Where(p => p.Id == id).FirstOrDefault();

            project.IsPublic = false;
            data.SaveChanges();
        }

        public void Delete(string id)
        {
            var project = this.data.Projects.Where(p => p.Id == id).FirstOrDefault();

            this.data.Projects.Remove(project);
            data.SaveChanges();
        }
    }
}
