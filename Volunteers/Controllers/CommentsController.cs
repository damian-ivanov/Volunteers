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
using Microsoft.AspNetCore.Authorization;
using Volunteers.Services.Comments;
using Volunteers.Models.Comments;

namespace Volunteers.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService comments;

        public CommentsController(ICommentService comments)
        {
            this.comments = comments;
        }


        [Authorize]
        [HttpPost]
        public IActionResult Add(string comment, string Id)
        {
            ViewData["comment"] = comment;

            comments.Add(comment, Id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            return Redirect("/Projects/Details/" + $"{Id}");
        }


    }
}

