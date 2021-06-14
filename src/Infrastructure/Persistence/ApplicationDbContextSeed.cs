using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var administratorRole = new IdentityRole("Administrator");

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
            }

            var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "qwerty12");
                await userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
            }
        }

        //public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        //{
        //    // Seed, if necessary
        //    if (!context.TodoLists.Any())
        //    {
        //        context.TodoLists.Add(new 
        //        {
        //            
        //        });

        //        await context.SaveChangesAsync();
        //    }
        //}
    }
}
