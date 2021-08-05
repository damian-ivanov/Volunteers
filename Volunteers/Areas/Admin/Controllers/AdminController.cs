namespace Volunteers.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Volunteers.Services.Stats;
    using static AdminConstants;

    [Area(AreaName)]
    [Authorize(Roles = AdministratorRoleName)]
    public abstract class AdminController : Controller
    {
        private readonly IStatsService stats;

        public AdminController(IStatsService stats)
        {
            this.stats = stats;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.ProjectsCount = stats.GetStats().Item1;
            ViewBag.CommentsCount = stats.GetStats().Item2;
            ViewBag.UsersCount = stats.GetStats().Item3;
            base.OnActionExecuting(filterContext);
        }
    }
}