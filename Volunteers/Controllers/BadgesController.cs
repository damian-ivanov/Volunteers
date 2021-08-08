using Microsoft.AspNetCore.Mvc;
using Volunteers.Services.Badges;

namespace Volunteers.Controllers
{
    public class BadgesController : Controller
    {
        private readonly IBadgesService badges;

        public BadgesController(IBadgesService badges)
        {
            this.badges = badges;
        }

        public IActionResult Index()
        {
            return View(badges.GetAllBadges());
        }
    }
}
