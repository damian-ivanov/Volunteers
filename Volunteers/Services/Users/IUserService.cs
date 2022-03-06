using System.Collections.Generic;
using System.Threading.Tasks;
using Volunteers.Models.Users;
using Volunteers.Services.Users.Models;

namespace Volunteers.Services.Users

{
    public interface IUserService
    {
        public bool IsOwner(string projectId, string userId);

        public Task<bool> IsAdministrator(string userId);

        public Task SetRole(string roleName, string userId);

        public Task<ProfileViewModel> GetUserInfo(string userName);

        public Task<List<ProfileViewModel>> AllUsersInfo(string projectId = null);

        public Task<bool> IsValid(string userName);

        public Task<IEnumerable<UsersServiceModel>> UsersList();

        public Task DeleteUser(string userId);

        public Task<ICollection<string>> GetRole(string userId);

    }
}