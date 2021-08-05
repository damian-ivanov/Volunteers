using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Volunteers.Data;
using Volunteers.Data.Models;
using Volunteers.Services.Users.Models;

namespace Volunteers.Services.Users

{
    public class UserService : IUserService
    {
        private readonly VolunteersDbContext data;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(VolunteersDbContext data, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.data = data;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public bool IsAdministrator(string userId)
        {
            throw new System.NotImplementedException();
        }

        public bool IsOwner(string projectId, string userId)
        {
            return this.data.Projects.Where(p => p.Id == projectId).Any(p => p.OwnerId == userId);
        }

        public async Task<IEnumerable<UsersServiceModel>> UsersList()
        {
            var usersList = new List<UsersServiceModel>();
            var users = this.data.Users;
            foreach (var user in users)
            {
                var role = await userManager.GetRolesAsync(await userManager.FindByIdAsync(user.Id));
                var userToAdd = new UsersServiceModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = role,
                    Username = user.UserName
                };
                usersList.Add(userToAdd);

            }
            return usersList;

            //return await this.data.Users.Select(u => new UsersServiceModel
            //{
            //    Id = u.Id,
            //    Email = u.Email,
            //    Role = GetRole(u.Id),
            //}).ToList();
        }

        public async Task<ICollection<string>> GetRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var role = await userManager.GetRolesAsync(user);
            return role.ToList();
        }
       
    }
}