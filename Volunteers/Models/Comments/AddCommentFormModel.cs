using System;
using System.ComponentModel.DataAnnotations;
using static Volunteers.Data.DataConstants;

namespace Volunteers.Models.Comments
{
    public class AddCommentFormModel
    {
        
        [MinLength(CommentMinLength)]
        [MaxLength(CommentMaxLength)]
        public string Content { get; set; }

        public string ProjectId { get; set; }

        public string UserName { get; set; }

        public DateTime PublishedOn { get; set; } = DateTime.Now;
    }
}
