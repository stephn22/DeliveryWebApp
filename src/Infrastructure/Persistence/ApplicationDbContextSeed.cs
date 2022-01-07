using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, IServiceProvider serviceProvider)
    {
        var administratorRole = new IdentityRole(RoleName.Admin);

        var config = serviceProvider.GetRequiredService<IConfiguration>();

        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
        }

        var administrator = new ApplicationUser
            { UserName = config["AdminCredentials:Email"], Email = config["AdminCredentials:Email"] };

        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, config["AdminCredentials:Password"]);
            await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });

            await userManager.SetLockoutEnabledAsync(administrator, false);

            var token = await userManager.GenerateEmailConfirmationTokenAsync(administrator);
            await userManager.ConfirmEmailAsync(administrator, token);
        }
    }
}