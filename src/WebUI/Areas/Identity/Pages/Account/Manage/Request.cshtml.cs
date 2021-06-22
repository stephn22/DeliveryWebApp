using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.WebUI.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Policy = PolicyName.IsClient)]
    public class RequestModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RequestModel> _logger;
        private readonly ApplicationDbContext _context;

        public RequestModel(UserManager<ApplicationUser> userManager, ILogger<RequestModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        public bool HasRequest { get; set; }

        public Request Request { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        [TempData] public string StatusMessage { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> Roles => new[]
        {
            new SelectListItem {Text = "Please select a role", Value = ""},
            new SelectListItem {Text = RoleName.Rider, Value = RoleName.Rider},
            new SelectListItem {Text = RoleName.Restaurateur, Value = RoleName.Restaurateur}
        };

        public class InputModel
        {
            [DataType(DataType.Text)] [Required] public string Role { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            await LoadRequestAsync(user);
            return Page();
        }

        private async Task LoadRequestAsync(ApplicationUser user)
        {
            try
            {
                Request = await (from r in _context.Requests
                    where r.Client.ApplicationUserFk == user.Id
                    select r).FirstOrDefaultAsync();
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning($"{e.Message} - {nameof(Request)}: cannot resolve user request");
                Request = null;
            }
            finally
            {
                HasRequest = (Request is not null);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!ModelState.IsValid)
            {
                await LoadRequestAsync(user);
                return Page();
            }

            return RedirectToPage();
        }
    }
}