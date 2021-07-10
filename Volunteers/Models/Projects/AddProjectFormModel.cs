using System;
using System.ComponentModel.DataAnnotations;
using static Volunteers.Data.DataConstants;

namespace Volunteers.Models.Projects
{
    public class AddProjectFormModel
    {
        [Required]
        [MinLength(ProjectTitleMinLength)]
        [MaxLength(ProjectTitleMaxLength)]
        public string  Title { get; set; }

        [Required]
        [MinLength(ProjectDescriptionMinLength)]
        [MaxLength(ProjectDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public string CategoryId { get; set; }

    }
}
