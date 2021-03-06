using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Volunteers.Services.Users.Models
{
    public class UsersServiceModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public ICollection<string> UserRoles { get; set; }

        public ICollection<IdentityRole> Roles { get; set; }
    }
}
