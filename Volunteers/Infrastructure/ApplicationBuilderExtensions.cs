namespace Volunteers.Infrastructure
{
    using System.Linq;
    using Volunteers.Data;
    using Volunteers.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<VolunteersDbContext>();

            data.Database.Migrate();

            SeedCategories(data);
            CreateAdminUser(data);
            //CreateRole(data, "Administrator");


            return app;
        }

        private static void SeedCategories(VolunteersDbContext data)
        {
            if (data.Categories.Any())
            {
                return;
            }

            data.Categories.AddRange(new[]
            {
                new Category { Name = "Cleaning", Description = "Test" },
                new Category { Name = "Ecology", Description = "Test" },
                new Category { Name = "Energy", Description = "Test" },
                new Category { Name = "Improvement" , Description = "Test" },
                new Category { Name = "Help" , Description = "Test" },
                new Category { Name = "Old people", Description = "Test" },
                new Category { Name = "Children" , Description = "Test" },
                new Category { Name = "Sponsorship" , Description = "Test" }
            });

            data.SaveChanges();
        }


        //private static void CreateRole(VolunteersDbContext data, string roleName)
        //{
        //    var role = new RoleStore<IdentityRole>(data);

        //    if (!data.Roles.Any(r => r.Name == roleName))
        //    {
        //        role.CreateAsync(new IdentityRole(roleName));
        //        data.SaveChanges();
        //    }

        //    if (!data.Roles.Any(r => r.Name == roleName))
        //    {
        //        var newRole = new IdentityRole { Name = roleName, NormalizedName = roleName };
        //        data.Roles.Add(newRole);
        //        data.SaveChanges();
        //    }
        //}

        private static void CreateAdminUser(VolunteersDbContext data)
        {

            if (data.Users.Any(u => u.UserName == "admin"))
            {
                return;
            }

            var adminUser = new User { Email = "admin@admin.com", PasswordHash = "123456", UserName = "admin" };
            

        }
    }
}