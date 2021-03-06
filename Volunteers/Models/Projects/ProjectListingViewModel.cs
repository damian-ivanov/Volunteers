namespace Volunteers.Models.Projects
{
    public class ProjectListingViewModel
    {
        public string Id { get; set; }

        public string  Title { get; set; }

        public string Description { get; set; }

        public string City { get; set; }

        public string Image { get; set; }

        public bool IsCompleted { get; set; }

        public string Address { get; set; }

        public string StartDate { get; set; }

        public string StartTime { get; set; }

        public string PublishedOn { get; set; }

        public int Participants { get; set; }

        public string Coordinates { get; set; }

        public bool Joined { get; set; }
    }
}
