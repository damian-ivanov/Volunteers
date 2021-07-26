using System.Linq;
using Volunteers.Data;
using static Volunteers.Data.DataConstants;

namespace Volunteers.Services.Users

{
    public class UserService : IUserService
    {
        private readonly VolunteersDbContext data;

        public UserService(VolunteersDbContext data)
        {
            this.data = data;
        }

        public bool IsAdministrator(string userId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsOwner(string projectId, string userId)
        {
            return this.data.Projects.Where(p => p.Id == projectId).Any(p => p.OwnerId == userId);
        }
    }
}