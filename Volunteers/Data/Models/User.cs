using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Volunteers.Data.Models
{
    public class User : IdentityUser
    {

        public ICollection<Project> Projects { get; set; } = new List<Project>();

        public virtual ICollection<Badge> Badges { get; set; } = new List<Badge>();

    }
}