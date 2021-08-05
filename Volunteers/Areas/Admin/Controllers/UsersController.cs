namespace Volunteers.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;
    using Volunteers.Data;
    using Volunteers.Models.Projects;
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


    }
}