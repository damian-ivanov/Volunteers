using System;

namespace Volunteers.Projects.Models
{
    public class ProjectListingViewModel
    {
        public string Id { get; set; }

        public string  Title { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string StartDate { get; set; }

        public string PublishedOn { get; set; }

        public int Votes { get; set; }

        public int Participants { get; set; }
    }
}
