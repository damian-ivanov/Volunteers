using System.Collections.Generic;
using Volunteers.Models.Badges;
using Volunteers.Models.Comments;

namespace Volunteers.Services.Badges

{
    public interface IBadgesService
    {
        public ICollection<BadgesListingViewModel> GetAllBadges();

        public void Evaluate(string userId);
    }
}