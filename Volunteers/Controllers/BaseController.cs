using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Volunteers.Controllers
{
    public class BaseController : Controller
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.NotificationsCount = "";
            base.OnActionExecuting(filterContext);
        }
    }
}
