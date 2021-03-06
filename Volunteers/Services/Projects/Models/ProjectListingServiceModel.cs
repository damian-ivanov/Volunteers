namespace Volunteers.Services.Projects.Models
{
    public class ProjectListingServiceModel
    {
        public string Id { get; set; }

        public string  Title { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public string Image { get; set; }

        public bool IsCompleted { get; set; }

        public string Address { get; set; }

        public string StartDate { get; set; }

        public string PublishedOn { get; set; }

        public int Participants { get; set; }

        public int Comments { get; set; }
    }
}
