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


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId != null)
            {
                var notificationsCount = notifications.GetNotifications(userId);

                ViewBag.NotificationsCount = notificationsCount;
                base.OnActionExecuting(filterContext);
            }
           
        }
    }
}
