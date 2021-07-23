﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volunteers.Data.Models;

namespace Volunteers.Data
{
    public class VolunteersDbContext : IdentityDbContext<User>
    {
        //public DbSet<User> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Badge> Badges { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Category> Categories { get; set; }

        public VolunteersDbContext(DbContextOptions<VolunteersDbContext> options)
            : base(options)
        {
        }
    }
}
