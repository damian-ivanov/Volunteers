using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Volunteers.Data.Models
{
    public class User : IdentityUser
    {
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        public ICollection<Badge> Badges { get; set; } = new List<Badge>();


    }
}