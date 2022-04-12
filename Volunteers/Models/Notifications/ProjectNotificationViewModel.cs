using System;

namespace Volunteers.Models.Notifications
{
    public class ProjectNotificationViewModel
    {
        public int Id { get; init; }

        public string ProjectId { get; set; }

        public string Title { get; set; }

        public DateTime PublishedOn { get; set; }

    }
}
