using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volunteers.Data.Models
{
    public class User : IdentityUser
    {
        public User()
        {
            this.Projects = new List<Project>();
            this.Badges = new List<Badge>();
        }

        public ICollection<Project> Projects { get; set; }

        public virtual ICollection<Badge> Badges { get; set; } 

    }
}