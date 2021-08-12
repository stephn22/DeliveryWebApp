using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        public static async Task BlockUser(this UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            var lockoutEndDate = new DateTime(2999, 01, 01);
            await userManager.SetLockoutEndDateAsync(user, lockoutEndDate);
        }

        public static async Task UnblockUser(this UserManager<ApplicationUser> userManager, ApplicationUser user)
        {
            await userManager.SetLockoutEndDateAsync(user, null);
        }

        public static IEnumerable<KeyValuePair<string, string>> CountryList()
        {
            var cultureList = new Dictionary<string, string>();

            var cultureInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            foreach (var c in cultureInfo)
            {
                var regionInfo = new RegionInfo(0x0409);
                try
                {
                    regionInfo = new RegionInfo(c.LCID);
                }
                catch (CultureNotFoundException e)
                {
                    regionInfo = new RegionInfo(0x0409); // English europe
                }
                finally
                {
                    if (!cultureList.ContainsKey(regionInfo.Name))
                    {
                        cultureList.Add(regionInfo.Name, regionInfo.EnglishName);
                    }
                }
            }
            // TODO: localize
            cultureList.Add("", "-- Select Country --");

            return cultureList.OrderBy(p => p.Value);
        }
    }
}