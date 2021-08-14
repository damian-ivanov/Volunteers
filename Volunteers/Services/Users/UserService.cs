using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Volunteers.Data;
using Volunteers.Data.Models;
using Volunteers.Models.Badges;
using Volunteers.Models.Projects;
using Volunteers.Models.Users;
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

        public async Task<bool> IsAdministrator(string editorId)
        {
            var user = await userManager.FindByIdAsync(editorId);
            return await userManager.IsInRoleAsync(user, DataConstants.AdministratorRoleName);
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
                    UserRoles = role,
                    Username = user.UserName,
                    Roles = roleManager.Roles.ToList()
                };
                usersList.Add(userToAdd);

            }
            return usersList;
        }

        public async Task<ICollection<string>> GetRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            var role = await userManager.GetRolesAsync(user);
            return role.ToList();
        }

        public async Task SetRole(string roleName, string userId)
        {
            var rolesToRemove = await userManager.GetRolesAsync(await userManager.FindByIdAsync(userId));
            var user = await userManager.FindByIdAsync(userId);

            await userManager.RemoveFromRolesAsync(user, rolesToRemove.ToArray());
            await userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<ProfileViewModel> GetUserInfo(string userName)
        {
            var currentUser = await this.data.Users.Include(b => b.Badges).Where(u => u.UserName == userName).FirstOrDefaultAsync();

            var role = await userManager.GetRolesAsync(currentUser);

            var projects = this.data.Projects.Where(p => p.IsPublic).AsQueryable();

            return new ProfileViewModel
            {
                Username = currentUser.UserName,
                Email = currentUser.Email,
                Role = role.FirstOrDefault(),
                ProjectsCompleted = projects.Where(p => p.IsCompleted && p.OwnerId == currentUser.Id).Count(),
                ProjectsInvolved = projects.Where(p => p.Users.Contains(currentUser)).Count(),
                ProjectsSubmitted = projects.Where(p => p.OwnerId == currentUser.Id).Count(),
                CommentsCount = this.data.Comments.Where(c => c.UserName == userName).Count(),
                BadgesEarned = currentUser.Badges.Select(b => new BadgesListingViewModel
                {
                    Description = b.Description,
                    Image = Path.Combine("/badges/", b.Image),
                    Title = b.Title,
                    Users = b.Users.Count()
                }),
                Projects = projects.Where(p => p.OwnerId == currentUser.Id).Select(p => new ProjectListingViewModel
                {
                    Address = p.Address,
                    City = p.City,
                    Description = p.Description,
                    Id = p.Id,
                    Participants = p.Users.Count(),
                    PublishedOn = p.PublishedOn.ToString("d"),
                    StartDate = p.StartDate.ToString("d"),
                    Title = p.Title,
                    IsCompleted = p.IsCompleted,
                    Image = Path.Combine("/uploads/", p.Image)
                }).ToList()
            };


        }

        public async Task DeleteUser(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            await userManager.DeleteAsync(user);
        }

        public async Task<bool> IsValid(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            return user == null ? false : true;
        }

        public List<ProfileViewModel> AllUsersInfo()
        {
            var projects = this.data.Projects.Where(p => p.IsPublic).AsQueryable();

            var users = this.data.Users.Select(u => new ProfileViewModel
            {
                Username = u.UserName,
                Email = u.Email,
                Role = userManager.GetRolesAsync(u).ToString(),
                ProjectsCompleted = projects.Where(p => p.IsCompleted && p.OwnerId == u.Id).Count(),
                ProjectsInvolved = projects.Where(p => p.Users.Contains(u)).Count(),
                ProjectsSubmitted = projects.Where(p => p.OwnerId == u.Id).Count(),
                CommentsCount = this.data.Comments.Where(c => c.User == u).Count(),
                DateJoined = u.RegistrationDate,
                LastLogin = u.LastLoginTime,
            }).ToList();

            return users;


        }
    }
}
