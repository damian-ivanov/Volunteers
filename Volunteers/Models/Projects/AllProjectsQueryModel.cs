using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volunteers.Data.Models;

namespace Volunteers.Models.Projects
{
    public class AllProjectsQueryModel
    {
        public const int ProjectsPerPage = 3;

        public int CurrentPage { get; init; } = 1;

        [Display(Name ="Location (optional")]
        public string SearchTerm { get; set; }

        public string Category { get; set; }

        public string SortOrder { get; set; }

        public bool ShowCompleted { get; set; }

        public string Image { get; set; }

        public int TotalProjects { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<ProjectListingViewModel> Projects { get; set; }
    }
}