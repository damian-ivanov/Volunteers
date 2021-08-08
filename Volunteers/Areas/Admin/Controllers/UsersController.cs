namespace Volunteers.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Volunteers.Services.Stats;
    using Volunteers.Services.Users;

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
            return RedirectToAction("Index", "Users");
        }

        public async Task<IActionResult> Delete(string Id)
        {
            await users.DeleteUser(Id);
            return RedirectToAction("Index", "Users");
        }


    }
}