using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Domain.Constants;
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
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RequestModel(UserManager<ApplicationUser> userManager, ILogger<RequestModel> logger,
            ApplicationDbContext context, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
        }

        public bool HasRequest { get; set; }

        public Request UserRequest { get; set; }
        public Client Client { get; set; }

        [BindProperty] public InputModel Input { get; set; }

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
                UserRequest = await (from r in _context.Requests
                    where r.Client.ApplicationUserFk == user.Id
                    select r).FirstOrDefaultAsync();
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning($"{e.Message} - {nameof(Request)}: cannot resolve user request");
                UserRequest = null;
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

            UserRequest = new Request
            {
                Client = await GetClientAsync(user),
                Role = Input.Role,
                Status = RequestStatus.Idle
            };

            _context.Requests.Add(UserRequest);

            await _context.SaveChangesAsync();
            await _signInManager.RefreshSignInAsync(user);

            _logger.LogInformation("User posted their request successfully.");

            return RedirectToPage();
        }

        private async Task<Client> GetClientAsync(ApplicationUser user)
        {
            return await (from c in _context.Clients
                where c.ApplicationUserFk == user.Id
                select c).FirstOrDefaultAsync();
        }
    }
}