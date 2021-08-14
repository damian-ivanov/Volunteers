using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Volunteers.Data.Models
{
    public class User : IdentityUser
    {

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public virtual ICollection<Badge> Badges { get; set; } = new List<Badge>();
        public virtual DateTime? LastLoginTime { get; set; }
        public virtual DateTime? RegistrationDate { get; init; }

    }
}