using System;
using System.ComponentModel.DataAnnotations;
using static Volunteers.Data.DataConstants;

namespace Volunteers.Data.Models
{
    public class Comment
    {
        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MinLength(CommentMinLength)]
        [MaxLength(CommentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string UserName { get; set; }

        public DateTime PublishedOn { get; set; } = DateTime.Now;

        public string UserId { get; set; }

        public User User { get; set; }

        public string ProjectId { get; set; }

        public Project Project { get; set; }

        public bool IsPublic { get; set; }
    }
}