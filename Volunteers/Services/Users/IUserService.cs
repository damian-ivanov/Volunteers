using System.Collections.Generic;
using System.Threading.Tasks;
using Volunteers.Data.Models;
using Volunteers.Services.Users.Models;

namespace Volunteers.Services.Users

{
    public interface IUserService
    {
        public bool IsOwner(string projectId, string userId);

        public bool IsAdministrator(string userId);

        public Task<IEnumerable<UsersServiceModel>> UsersList();
    }
}