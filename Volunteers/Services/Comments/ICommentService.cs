using System.Collections.Generic;
using Volunteers.Models.Comments;

namespace Volunteers.Services.Comments

{
    public interface ICommentService
    {
        string Add(string content, string ProjectId, string UserId);

        public ICollection<CommentListingViewModel> GetUnapprovedComments();

        public void Delete(string Id);

        public void Approve(string Id);
    }
}