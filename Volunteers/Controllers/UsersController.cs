using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volunteers.Services.Notifications;
using Volunteers.Services.Users;

namespace Volunteers.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserService users;

        public UsersController(INotificationsService notifications, IUserService users) : base(notifications)
        {
            this.users = users;
        }

        public async Task<IActionResult> Profile(string Id)
        {
            if (!await users.IsValid(Id))
            {
                return RedirectToAction("Error", "Home");
            }

            return View(await users.GetUserInfo(Id));
        }

        public async Task<IActionResult> Participants()
        {

            return View(await users.AllUsersInfo());
        }
    }
}

