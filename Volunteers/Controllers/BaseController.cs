using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using System.Threading.Tasks;
using Volunteers.Services.Notifications;


namespace Volunteers.Controllers
{
    public class BaseController : Controller
    {
        public INotificationsService notifications;

        public BaseController(INotificationsService notifications)
        {
            this.notifications = notifications;
        }


        [Authorize]
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                //var notificationsCount = notifications.GetNotifications(userId);
                
                
                var notificationsCount = Task.Run(async () => await notifications.GetNotifications(userId)).Result;
                ViewBag.NotificationsCount = notificationsCount;
                base.OnActionExecuting(filterContext);
            }

        }
    }
}
