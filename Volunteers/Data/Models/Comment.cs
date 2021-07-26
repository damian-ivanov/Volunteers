using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volunteers.Data.Models
{
    public class Comment
    {
        [Key]
        [Required]
        public int Id { get; init; }

        [Required]
        [MaxLength(1000)]
        public string Content { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public string ProjectId { get; set; }

        public Project Project { get; set; }
    }
}