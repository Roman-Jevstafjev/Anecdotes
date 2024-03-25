using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Jevstafjev.Anecdotes.Infrastructure.DatabaseInitialization
{
    public static class DatabaseInitializer
    {
        public static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            using var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            if (context is null)
            {
                return;
            }

            var pending = context.Database.GetPendingMigrations();
            if (pending.Any())
            {
                await context.Database.MigrateAsync();
            }

            using var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            if (userManager is null)
            {
                return;
            }

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Administrator", NormalizedName = "Administrator".ToUpper() });
            }

            const string UserName1 = "administrator@gmail.com";
            if (await userManager.FindByNameAsync(UserName1) is null)
            {
                var user = new ApplicationUser
                {
                    UserName = UserName1,
                    Email = UserName1,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "Password");
                await userManager.AddToRoleAsync(user, "Administrator");
            }
        }
    }
}
