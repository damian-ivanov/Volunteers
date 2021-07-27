using System.Linq;
using Volunteers.Data;
using Volunteers.Data.Models;
using Volunteers.Services.Comments;
using static Volunteers.Data.DataConstants;

namespace Volunteers.Services.Users

{
    public class CommentService : ICommentService
    {
        private readonly VolunteersDbContext data;

        public CommentService(VolunteersDbContext data)
        {
            this.data = data;
        }

        public string Add(string content, string ProjectId, string UserId)
        {
            var commentData = new Comment
            {
                Content = content,
                ProjectId = ProjectId,
                UserId = UserId,
                UserName = data.Users.Where(u => u.Id == UserId).Select(u => u.UserName).FirstOrDefault(),
                Project = data.Projects.Where(p => p.Id == ProjectId).FirstOrDefault(),
                User = data.Users.Where(u => u.Id == UserId).FirstOrDefault()

            };

            this.data.Comments.Add(commentData);
            var project = this.data.Projects.Where(p => p.Id == ProjectId).FirstOrDefault();
            project.Comments.Add(commentData);
            this.data.SaveChanges();

            return commentData.Id;
        }
    }
}