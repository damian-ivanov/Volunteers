using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volunteers.Services.Projects;
using Volunteers.Services.Users;

namespace Volunteers.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly IProjectService projects;

        public NotificationsController(IProjectService projects)
        {
            this.projects = projects;
        }

        public async Task<IActionResult> Index(string Id)
        {

            return View(await projects.GetNotifications(Id));
        }
    }
}

