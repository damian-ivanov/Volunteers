using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Volunteers.Data;
using Volunteers.Models.Badges;
using Volunteers.Models.Comments;
using static Volunteers.Data.DataConstants;

namespace Volunteers.Services.Badges

{
    public class BadgesService : IBadgesService
    {
        private readonly VolunteersDbContext data;

        public BadgesService(VolunteersDbContext data)
        {
            this.data = data;
        }

        public void Evaluate(string userId)
        {
            var projectsOwned = this.data.Projects.Where(o => o.OwnerId == userId && o.IsPublic).Count();
            string title;

            if (projectsOwned ==  0)
            {
                return;
            }
            else if (projectsOwned >= 1 && projectsOwned < 3)
            {
                title = FirstBadgeTitle;
            }
            else if (projectsOwned >= 3 && projectsOwned < 5)
            {
                title = SecondBadgeTitle;
            }
            else if (projectsOwned >= 5 && projectsOwned < 10)
            {
                title = ThirdBadgeTitle;
            }
            else if (projectsOwned >= 10)
            {
                title = ForthBadgeTitle;
            }
            else
            {
                return;
            }

            var currentUser = this.data.Users.Include(p => p.Badges).FirstOrDefault(u => u.Id == userId);
            var badgeToAdd = this.data.Badges.Where(b => b.Title == title).FirstOrDefault();
            
            if (!currentUser.Badges.Contains(badgeToAdd))
            {
                currentUser.Badges.Add(badgeToAdd);
                this.data.SaveChanges();
            }
            
        }

        public ICollection<BadgesListingViewModel> GetAllBadges()
        {
            return this.data.Badges.Select(b => new BadgesListingViewModel
            {
                Description = b.Description,
                Image = Path.Combine("/badges/", b.Image),
                Users = b.Users.Count
            }).OrderByDescending(u => u.Users).ToList();
        }
    }
}