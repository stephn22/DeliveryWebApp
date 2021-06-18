using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DeliveryWebApp.Infrastructure.Security;

namespace DeliveryWebApp.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var administratorRole = new IdentityRole("Administrator");

            const string adminUserName = "administrator@localhost";

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
            }

            var administrator = new ApplicationUser { UserName = adminUserName, Email = adminUserName };

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "Administrator1!");

                if (await userManager.FindByIdAsync(administrator.Id) != null)
                {
                    await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });

                    await userManager.AddClaimAsync(administrator, new Claim(ClaimName.Role, RoleName.Admin));
                }

            }
        }
    }
}
