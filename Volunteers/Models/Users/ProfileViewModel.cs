using System.Collections.Generic;
using Volunteers.Models.Badges;
using Volunteers.Models.Projects;

namespace Volunteers.Models.Users
{
    public class ProfileViewModel
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public int ProjectsSubmitted { get; set; }

        public int ProjectsCompleted { get; set; }

        public int ProjectsInvolved { get; set; }

        public int CommentsCount { get; set; }

        public IEnumerable<ProjectListingViewModel> Projects { get; set; }

        public IEnumerable<BadgesListingViewModel> BadgesEarned { get; set; }
    }
}