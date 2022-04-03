using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Dynamic;
using System.Threading.Tasks;

namespace Volunteers.Models
{
    public class NotificationStatic : ActionFilterAttribute
    {
        //public void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    filterContext.Controller.ViewBag.Company = "Company";
        //}

    }
}
