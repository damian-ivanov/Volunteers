using System.Collections.Generic;
using System.Linq;
using Volunteers.Data;
using Volunteers.Data.Models;
using Volunteers.Models.Comments;
using Volunteers.Services.Comments;

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

        public void Approve(string Id)
        {
            var comment = this.data.Comments.Where(c => c.Id == Id).FirstOrDefault();
            comment.IsPublic = true;
            this.data.SaveChanges();
        }

        public void Delete(string Id)
        {
            var comment = this.data.Comments.Where(c => c.Id == Id).FirstOrDefault();
            this.data.Comments.Remove(comment);
            this.data.SaveChanges();
           
        }

        public ICollection<CommentListingViewModel> GetUnapprovedComments()
        {
            return data.Comments.Where(p => p.IsPublic == false).Select(p => new CommentListingViewModel
            {
                Id = p.Id,
                ProjectId = p.ProjectId,
                Content = p.Content,
                PublishedOn = p.PublishedOn.ToString("d"),
                UserName = p.UserName
            }).ToList();
        }
    }

}