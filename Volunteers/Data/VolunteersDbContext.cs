using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Volunteers.Data.Models;

namespace Volunteers.Data
{
    public class VolunteersDbContext : IdentityDbContext<User>
    {


        public DbSet<Project> Projects { get; set; }

        public DbSet<Badge> Badges { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Category> Categories { get; set; }

        public VolunteersDbContext(DbContextOptions<VolunteersDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)

        {
            builder.Entity<Project>().HasMany<Comment>(c => c.Comments).WithOne(p => p.Project).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Project>().HasMany(x => x.Users).WithMany(x => x.Projects);
            base.OnModelCreating(builder);
        }


    }
}

//modelBuilder.Entity<Parent>()
//              .HasMany<Child>(c => c.Children)
//              .WithOptional(x => x.Parent)
//              .WillCascadeOnDelete(true);

//Builder.Entity<OfficerPrisoner>()
//.HasOne(x => x.Prisoner)
//.WithMany(x => x.PrisonerOfficers)
//.HasForeignKey(x => x.PrisonerId)
//.OnDelete(DeleteBehavior.Restrict);
