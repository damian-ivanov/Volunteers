using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Volunteers.Data.DataConstants;

namespace Volunteers.Models.Comments
{
    public class AddCommentFormModel
    {
        [Required]
        [MinLength(CommentMinLength)]
        [MaxLength(CommentMaxLength)]
        public string Content { get; set; }

    }
}
