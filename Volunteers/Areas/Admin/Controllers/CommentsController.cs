namespace Volunteers.Areas.Admin.Controllers
{

    using Microsoft.AspNetCore.Mvc;
    using Volunteers.Services.Comments;
    using Volunteers.Services.Stats;

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
            return RedirectToAction("Index", "Comments");
        }

        public IActionResult Approve(string Id)
        {
            comments.Approve(Id);
            return RedirectToAction("Index", "Comments");
        }
    }
}