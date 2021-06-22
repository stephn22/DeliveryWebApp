using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.WebUI.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Policy = PolicyName.IsClient)]
    public class AddressModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AddressModel> _logger;
        private readonly ApplicationDbContext _context;

        public AddressModel(UserManager<ApplicationUser> userManager, ILogger<AddressModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        [TempData] public string StatusMessage { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public IQueryable<ICollection<Address>> Addresses { get; set; }

        public SelectList Countries => new(CountryList(), "Key", "Value");
        public bool HasAtLeastOneAddress { get; set; }
        public bool Edit { get; set; }
        public bool AddNew { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Address Line 1")]
            public string AddressLine1 { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Address Line 2")]
            public string AddressLine2 { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Number")]
            public string Number { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "City")]
            public string City { get; set; }

            [Required]
            [DataType(DataType.PostalCode)]
            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "State or Province")]
            public string Country { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            LoadAddresses(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!ModelState.IsValid)
            {
                LoadAddresses(user);
                return Page();
            }

            StatusMessage = "Your addresses have been updated";
            return RedirectToPage();
        }

        private IEnumerable<KeyValuePair<string, string>> CountryList()
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
                    _logger.LogWarning($"{e.Message}: culture not found, using English(0x0409) by default");
                }
                finally
                {
                    if (!cultureList.ContainsKey(regionInfo.Name))
                    {
                        cultureList.Add(regionInfo.Name, regionInfo.EnglishName);
                    }
                }
            }

            return cultureList.OrderBy(p => p.Value);
        }

        private void LoadAddresses(ApplicationUser user)
        {
            try
            {
                Addresses = from c in _context.Clients
                    where user.Id == c.ApplicationUserFk
                    select c.Addresses;
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning($"{e.Message} - {nameof(Addresses)}: cannot resolve user addresses");
                Addresses = null;
            }
            finally
            {
                HasAtLeastOneAddress = (!Addresses.IsNullOrEmpty());
            }
        }
    }
}