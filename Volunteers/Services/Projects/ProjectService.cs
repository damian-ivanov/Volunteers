using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volunteers.Areas.Admin.Models;
using Volunteers.Data;
using Volunteers.Data.Models;
using Volunteers.Models.Notifications;
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

        public IEnumerable<ProjectListingAdminViewModel> ListProjectsAdmin()
        {
            return data.Projects.Where(p => p.IsPublic == false).Select(p => new ProjectListingAdminViewModel
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
                OwnerId = ownerId,
                Coordinates = project.Coordinates.Substring(7).TrimEnd(')')
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
                Joined = p.Users.Contains(this.data.Users.Where(u => u.Id == userId).FirstOrDefault()),
                Coordinates = p.Coordinates
            }).FirstOrDefault();
        }

        public EditProjectViewModel Edit(string id)
        {
            var project = this.data.Projects.Where(p => p.Id == id).AsQueryable();
            return project.Select(p => new EditProjectViewModel
            {
                Address = p.Address,
                CurrentCategory = p.Category.Name,
                CurrentCategoryId = p.CategoryId,
                City = p.City,
                Description = p.Description,
                StartDate = p.StartDate,
                Title = p.Title,
                Coordinates = "LatLng(" + p.Coordinates + ")",
                OldImage = Path.Combine("/uploads/", p.Image),
                Image = Path.Combine("/uploads/", p.Image)
            }).FirstOrDefault();
        }

        public async Task<bool> Edit(EditProjectViewModel project, string secureImageName, string editorId)
        {
            var projectToEdit = this.data.Projects.SingleOrDefault(p => p.Id == project.Id);
            var isOwnerAdmin = await userService.IsAdministrator(editorId);

            projectToEdit.Coordinates = project.Coordinates.Substring(7).TrimEnd(')');
            projectToEdit.Address = project.Address;
            projectToEdit.CategoryId = project.CategoryId;
            projectToEdit.City = project.City;
            projectToEdit.Description = project.Description;
            projectToEdit.StartDate = project.StartDate;
            projectToEdit.Title = project.Title;
            projectToEdit.Image = secureImageName;

            if (!isOwnerAdmin)
            {
                projectToEdit.IsPublic = false;
            }

            data.SaveChanges();

            if (isOwnerAdmin)
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public bool Approve(string id, string userId)
        {
            var project = this.data.Projects.Where(p => p.Id == id).FirstOrDefault();

            if (project == null)
            {
                return false;
            }

            project.IsPublic = true;

            if (!this.data.Notifications.Any(n => n.ProjectId == id))
            {
                var notification = new Notification
                {
                    ProjectId = project.Id,
                    Title = project.Title,
                    PublishedOn = project.PublishedOn,
                };

                data.Notifications.Add(notification);
                data.SaveChanges();
            }


            RemoveFromNotifications(id, userId);
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

        public Project Join(string id, string userId)
        {
            var project = this.data.Projects.Include(u => u.Users).FirstOrDefault(p => p.Id == id);
            var user = this.data.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (project.Users.Contains(user))
            {
                return project;
            }

            project.Users.Add(user);

            data.SaveChanges();
            return project;
        }

        public Project Leave(string id, string userId)
        {
            var project = this.data.Projects.Include(u => u.Users).FirstOrDefault(p => p.Id == id);
            var user = this.data.Users.Where(u => u.Id == userId).FirstOrDefault();

            if (project == null || user == null)
            {
                return project;
            }

            if (!project.Users.Contains(user))
            {
                return project;
            }

            project.Users.Remove(user);

            data.SaveChanges();
            return project;
        }

        public IEnumerable<ProjectCategoryViewModel> GetProjectCategories()
        {
            return this.data.Categories.Select(c => new ProjectCategoryViewModel
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        public bool EmptyCategoryCheck(AddProjectFormModel project)
        {
            return this.data.Categories.Any(c => c.Id == project.CategoryId);
        }

        public bool EmptyCategoryCheck(EditProjectViewModel project)
        {
            return this.data.Categories.Any(c => c.Id == project.CategoryId);
        }

        public IEnumerable<ProjectListingViewModel> ListProjectsHomePage(AllProjectsQueryModel query, string userId)
        {
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
                    StartTime = p.StartDate.ToString("HH:mm"),
                    Title = p.Title,
                    Image = Path.Combine("/uploads/", p.Image),
                    IsCompleted = p.IsCompleted,
                    Coordinates = p.Coordinates,
                    Joined = p.Users.Contains(this.data.Users.Where(u => u.Id == userId).FirstOrDefault()),
                }).ToList();

            query.Categories = categories;
            query.Projects = projects;
            query.TotalProjects = totalProjects;

            return projects;
        }

        public bool IsValid(string id)
        {
            if (this.data.Projects.Where(p => p.Id == id).Any())
            {
                return true;
            }

            return false;
        }


        public void RemoveFromNotifications(string projectId, string userId)
        {
            var user = this.data.Users.FirstOrDefault(u => u.Id == userId);
            var notification = this.data.Notifications.Include(u => u.Users).Where(n => n.ProjectId == projectId).FirstOrDefault();
            notification.Users.Add(user);
            this.data.SaveChanges();
        }
    }
}
