using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volunteers.Data;
using Volunteers.Models;
using Volunteers.Models.Projects;
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
