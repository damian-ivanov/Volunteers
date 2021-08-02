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
    using System.Threading.Tasks;
    using static Volunteers.Data.DataConstants;
    using System;

    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var services = serviceScope.ServiceProvider;

            MigrateDatabase(services);

            SeedCategories(services);
            SeedAdministrator(services);
            SeedBadges(services);

            return app;
        }

        private static void SeedBadges(IServiceProvider services)
        {
            var data = services.GetRequiredService<VolunteersDbContext>();
            if (data.Badges.Any())
            {
                return;
            }

            data.Badges.AddRange(new[]
            {
                new Badge {  Title=FirstBadgeTitle, Description = FirstBadgeDesctiption, Image = FirstBadgeImage},
                new Badge {  Title=SecondBadgeTitle, Description = SecondBadgeDesctiption, Image = SecondBadgeImage},
                new Badge {  Title=ThirdBadgeTitle, Description = ThirdBadgeDesctiption, Image = ThirdBadgeImage},
                new Badge {  Title=ForthBadgeTitle, Description = ForthBadgeDesctiption, Image = ForthBadgeImage},
            });

            data.SaveChanges();
        }

        private static void MigrateDatabase(IServiceProvider services)
        {
            var data = services.GetRequiredService<VolunteersDbContext>();

            data.Database.Migrate();
        }

        private static void SeedCategories(IServiceProvider services)
        {
            var data = services.GetRequiredService<VolunteersDbContext>();
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


        private static void SeedAdministrator(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<User>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = AdministratorRoleName };

                    await roleManager.CreateAsync(role);
         
                    var user = new User
                    {
                        Email = AdministratorEmail,
                        UserName = AdministratorUsername,
                        
                    };

                    await userManager.CreateAsync(user, AdministratorPassword);
                    await userManager.AddToRoleAsync(user, role.Name);
                })
                .GetAwaiter()
                .GetResult();
        }
    }
}