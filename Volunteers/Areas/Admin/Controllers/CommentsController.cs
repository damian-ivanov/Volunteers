namespace Volunteers.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Threading.Tasks;
    using Volunteers.Data;
    using Volunteers.Models.Projects;
    using Volunteers.Services.Comments;
    using Volunteers.Services.Stats;
    using Volunteers.Services.Users;

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