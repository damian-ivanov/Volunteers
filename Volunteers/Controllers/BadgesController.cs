using Microsoft.AspNetCore.Mvc;
using Volunteers.Services.Badges;
using Volunteers.Services.Notifications;

namespace Volunteers.Controllers
{
    public class BadgesController : BaseController
    {
        private readonly IBadgesService badges;

        public BadgesController(INotificationsService notifications, IBadgesService badges) : base(notifications)
        {
            this.badges = badges;
        }

        public IActionResult Index()
        {
            return View(badges.GetAllBadges());
        }
    }
}
