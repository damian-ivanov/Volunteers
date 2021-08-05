using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volunteers.Services.Users.Models
{
    public class UsersServiceModel
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public ICollection<string> Role { get; set; }
    }
}
