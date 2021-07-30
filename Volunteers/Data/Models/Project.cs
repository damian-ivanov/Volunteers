using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volunteers.Data.Models
{
    public class Project
    {
        public Project()
        {
            this.Users = new List<User>();
            this.Comments = new List<Comment>();
        }

        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(50)]
        public string City { get; set; }

        [Required]
        [MaxLength(50)]
        public string Address { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public int Votes { get; set; }

        public string Image { get; set; }

        public DateTime PublishedOn { get; set; } = DateTime.Now;

        public bool IsPublic { get; set; }

        public bool IsCompleted { get; set; }

        public string OwnerId { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }


    }
}
