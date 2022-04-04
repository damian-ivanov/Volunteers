using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volunteers.Services.Notifications;
using Volunteers.Services.Projects;
using Volunteers.Services.Users;

namespace Volunteers.Controllers
{
    public class NotificationsController : BaseController
    {
        private readonly IProjectService projects;


        public NotificationsController(INotificationsService notifications, IProjectService projects) : base(notifications)
        {
            this.projects = projects;
        }

        public async Task<IActionResult> Index(string Id)
        {

            return View(await projects.GetNotifications(Id));
        }
    }
}

