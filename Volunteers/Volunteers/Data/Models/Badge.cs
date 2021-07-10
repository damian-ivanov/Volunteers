using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volunteers.Data.Models
{
    public class Badge
    {
        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [MaxLength(50)]
        public string Description { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();

    }
}