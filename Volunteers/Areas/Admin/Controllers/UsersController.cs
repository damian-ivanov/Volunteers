using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volunteers.Services.Stats;
using Volunteers.Services.Users;
using static Volunteers.WebConstants;

namespace Volunteers.Areas.Admin.Controllers
{
    public class UsersController : AdminController
    {
        private readonly IUserService users;

        public UsersController(IStatsService stats, IUserService users) : base(stats)
        {
            this.users = users;
        }

        public async Task<IActionResult> Index()
        {
            return View(await users.UsersList());
        }

        public async Task<IActionResult> SetRole(string roleName, string Id)
        {
            await users.SetRole(roleName, Id);
            TempData[GlobalMessageKey] = SetRoles;
            return RedirectToAction("Index", "Users");
        }

        public async Task<IActionResult> Delete(string Id)
        {
            await users.DeleteUser(Id);
            TempData[GlobalMessageKey] = DeleteUser;
            return RedirectToAction("Index", "Users");
        }


    }
}