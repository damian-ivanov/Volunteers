namespace Volunteers.Infrastructure
{
    using System.Linq;
    using Volunteers.Data;
    using Volunteers.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.AspNetCore.Identity;

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
                new Category { Name = "Improvement" , Description = "Test" },
                new Category { Name = "Help" , Description = "Test" },
                new Category { Name = "Old people", Description = "Test" },
                new Category { Name = "Children" , Description = "Test" },
                new Category { Name = "Sponsorship" , Description = "Test" }
            });

            data.SaveChanges();
        }

        private static void CreateAdminUser(VolunteersDbContext data)
        {

            if (data.Users.Any(u => u.UserName == "admin"))
            {
                return;
            }

            data.Users.Add(new User { Email = "damian.i.ivanov@gmail.com", PasswordHash = "123456", UserName = "admin"});
            
        }
    }
}