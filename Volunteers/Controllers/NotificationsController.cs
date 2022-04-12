using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volunteers.Services.Notifications;
using Volunteers.Services.Projects;
using Volunteers.Services.Users;

namespace Volunteers.Controllers
{
    public class NotificationsController : BaseController
    {
        public NotificationsController(INotificationsService notifications) : base(notifications)
        {
        }

        public async Task<IActionResult> Index(string Id)
        {

            return View(await notifications.ListNotifications(Id));
        }
    }
}

