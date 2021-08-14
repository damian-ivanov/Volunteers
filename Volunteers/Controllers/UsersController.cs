using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volunteers.Services.Users;

namespace Volunteers.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService users;

        public UsersController(IUserService users)
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

        public IActionResult Participants()
        {

            return View(users.AllUsersInfo());
        }
    }
}

