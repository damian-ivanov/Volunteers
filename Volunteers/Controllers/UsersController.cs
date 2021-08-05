using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Volunteers.Data;
using Volunteers.Data.Models;
using Volunteers.Models.Projects;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
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
            return View(await users.GetUserInfo(Id));
        }
    }
}

