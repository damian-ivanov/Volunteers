using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static Volunteers.Data.DataConstants;

namespace Volunteers.Data.Models
{
    public class Notification
    {
        [Key]
        [Required]
        public int Id { get; init; }

        [Required]
        public string ProjectId { get; set; }

        [Required]
        public string Title { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();


    }
}