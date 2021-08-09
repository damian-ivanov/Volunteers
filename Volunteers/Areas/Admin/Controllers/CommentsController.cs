using Microsoft.AspNetCore.Mvc;
using Volunteers.Services.Comments;
using Volunteers.Services.Stats;
using static Volunteers.WebConstants;

namespace Volunteers.Areas.Admin.Controllers
{

    public class CommentsController : AdminController
    {
        private readonly ICommentService comments;

        public CommentsController(IStatsService stats, ICommentService comments) : base(stats)
        {
            this.comments = comments;
        }

        public IActionResult Index()
        {
            return View(comments.GetUnapprovedComments());
        }


        public IActionResult Delete(string Id)
        {
            comments.Delete(Id);
            TempData[GlobalMessageKey] = DeletedComment;
            return RedirectToAction("Index", "Comments");
        }

        public IActionResult Approve(string Id)
        {
            comments.Approve(Id);
            TempData[GlobalMessageKey] = ApprovedComment;
            return RedirectToAction("Index", "Comments");
        }
    }
}