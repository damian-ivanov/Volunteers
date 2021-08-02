using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Volunteers.Data.DataConstants;

namespace Volunteers.Data.Models
{
    public class Badge
    {
        [Key]
        [Required]
        public int Id { get; init; }

        [Required]
        [MaxLength(BadgesTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(BadgesDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string Image { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();

    }
}