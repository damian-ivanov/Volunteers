using System.Collections.Generic;
using Volunteers.Models.Badges;

namespace Volunteers.Services.Badges

{
    public interface IBadgesService
    {
        public ICollection<BadgesListingViewModel> GetAllBadges();

        public void Evaluate(string userId);
    }
}