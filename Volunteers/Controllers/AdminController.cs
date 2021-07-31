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

namespace Volunteers.Controllers
{
    public class AdminController : Controller
    {
        private readonly VolunteersDbContext data;

        public AdminController(VolunteersDbContext data)
        {
            this.data = data;
        }

        public IActionResult Admin()
        {
            return View(data.Projects.Where(p => p.IsPublic == false).Select(p => new ProjectListingViewModel
            {
                Address = p.Address,
                City = p.City,
                Description = p.Description,
                Id = p.Id,
                Participants = p.Users.Count(),
                PublishedOn = p.PublishedOn.ToString("d"),
                StartDate = p.StartDate.ToString("d"),
                Title = p.Title, 
            }).ToList());
        }





    }
    }

