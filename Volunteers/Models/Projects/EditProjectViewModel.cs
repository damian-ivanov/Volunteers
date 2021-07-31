using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volunteers.Data.Models;
using static Volunteers.Data.DataConstants;


namespace Volunteers.Models.Projects
{
    public class EditProjectViewModel
    {
        public string Id { get; set; }

        [Required]
        [MinLength(ProjectTitleMinLength)]
        [MaxLength(ProjectTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(ProjectDescriptionMinLength)]
        [MaxLength(ProjectDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        public string  OldImage { get; set; }

        [Display(Name = "Upload a new photo")]
        public string Image { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public string CurrentCategory { get; set; }

        public int CurrentCategoryId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; init; }
        public IEnumerable<ProjectCategoryViewModel> Categories { get; set; }

    }
}
