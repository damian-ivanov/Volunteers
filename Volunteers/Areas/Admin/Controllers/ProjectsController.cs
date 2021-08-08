namespace Volunteers.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using Volunteers.Data;
    using Volunteers.Models.Projects;
    using Volunteers.Services.Projects;
    using Volunteers.Services.Stats;

    public class ProjectsController : AdminController
    {
        private readonly IProjectService projects;

        public ProjectsController(IStatsService stats, IProjectService projects) : base(stats)
        {
            this.projects = projects;
        }

        public IActionResult Index()
        {
            return View(projects.ListProjectsAdmin());
        }


    }
}