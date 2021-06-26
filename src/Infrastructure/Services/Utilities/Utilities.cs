using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryWebApp.Infrastructure.Services.Utilities
{
    public static class Utilities
    {
        public static async Task<string> GetRoleAsync(this UserManager<ApplicationUser> userManager,
            ApplicationUser user)
        {
            var claims = await userManager.GetClaimsAsync(user);
            var claim = claims.First(u => u.Type == ClaimName.Role);

            return claim.Value;
        }

        public static async Task<Claim> GetClaimByTypeAsync(this UserManager<ApplicationUser> userManager,
            ApplicationUser user, string claimType)
        {
            var c = (from claim in await userManager.GetClaimsAsync(user)
                where claim.Type == claimType
                select claim).First();

            return c;
        }

        public static async Task<string> GetFNameAsync(this ApplicationUser user,
            UserManager<ApplicationUser> userManager)
        {
            if (user == null)
            {
                return "";
            }

            var fName = (from claim in await userManager.GetClaimsAsync(user)
                where claim.Type == ClaimName.FName
                select claim.Value).First();

            return fName;
        }

        public static async Task<string> GetLNameAsync(this ApplicationUser user,
            UserManager<ApplicationUser> userManager)
        {
            if (user == null)
            {
                return "";
            }

            var lName = (from claim in await userManager.GetClaimsAsync(user)
                where claim.Type == ClaimName.LName
                select claim.Value).First();

            return lName;
        }

        public static async Task<Claim> GetEnabledClaimAsync(this UserManager<ApplicationUser> userManager,
            ApplicationUser user)
        {
            var claim = (from c in await userManager.GetClaimsAsync(user)
                where c.Type == ClaimName.Enabled
                select c).First();

            return claim;
        }
    }
}