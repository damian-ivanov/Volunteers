using System;
using System.Collections.Generic;
using Volunteers.Data.Models;

namespace Volunteers.Models.Projects
{
    public class ProjectDetailViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public string Image { get; set; }

        public string Address { get; set; }

        public string StartDate { get; set; }

        public string PublishedOn { get; set; }

        public bool IsPublic { get; set; }

        public int Votes { get; set; }

        public int Participants { get; set; }

        public string Category { get; set; }

        public string Owner { get; set; }

        public bool IsCompleted { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
