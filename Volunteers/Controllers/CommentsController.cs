﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Volunteers.Services.Comments;
using static Volunteers.WebConstants;
using Volunteers.Services.Notifications;

namespace Volunteers.Controllers
{
    public class CommentsController : BaseController
    {
        private readonly ICommentService comments;

        public CommentsController(ICommentService comments, INotificationsService notifications) : base(notifications)
        {
            this.comments = comments;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Add(string comment, string Id)
        {
            ViewData["comment"] = comment;

            comments.Add(comment, Id, User.FindFirstValue(ClaimTypes.NameIdentifier));
            TempData[GlobalMessageKey] = PostedComment;
            return Redirect("/Projects/Details/" + $"{Id}");
        }

        
    }
}

