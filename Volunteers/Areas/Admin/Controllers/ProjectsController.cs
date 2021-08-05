namespace Volunteers.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using Volunteers.Data;
    using Volunteers.Models.Projects;
    using Volunteers.Services.Stats;

    public class ProjectsController : AdminController
    {
        private readonly VolunteersDbContext data;

        public ProjectsController(IStatsService stats, VolunteersDbContext data) : base(stats)
        {
            this.data = data;
        }

        public IActionResult Index()
        {
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


    }
}