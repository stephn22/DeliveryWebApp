using System;
using System.ComponentModel.DataAnnotations;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace DeliveryWebApp.WebUI.Pages.Admin
{
    [Authorize(Roles = RoleName.Admin)]
    public class RequestDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RequestDetailModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Request UserRequest { get; set; }
        public ApplicationUser Client { get; set; }
        public string ClaimValue { get; set; }
        public bool IsRider { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "DeliveryCredit")]
            public double DeliveryCredit { get; set; } // TODO: Culture
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // get request instance
            UserRequest = await _context.Requests.FirstOrDefaultAsync(o => o.Id == id);

            var appUserFk = UserRequest.Client.ApplicationUserFk;

            Client = await GetUserAsync(appUserFk);
            await GetClaimAsync(Client);

            IsRider = (UserRequest.Role == RoleName.Rider);

            if (Request == null)
            {
                return NotFound();
            }

            return Page();
        }

        private async Task<ApplicationUser> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        private async Task GetClaimAsync(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);

            var claim = claims.First(u => u.Type == ClaimName.Role);

            ClaimValue = claim.Value;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // update tables
            if (IsRider)
            {
                _context.Riders.Add(new Rider()
                {
                    Client = UserRequest.Client,
                    DeliveryCredit = Input.DeliveryCredit
                });
            }
            else
            {
                _context.Restaurateurs.Add(new Restaurateur()
                {
                    Client = UserRequest.Client,
                });
            }

            // TODO push notification to client

            await _context.SaveChangesAsync();
            return RedirectToPage("/Admin/Requests");
        }
    }
}
