using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace DeliveryWebApp.WebUI.Pages.Admin
{
    [Authorize(Roles = RoleName.Admin)]
    public class RequestDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RequestDetailModel> _logger;

        public RequestDetailModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<RequestDetailModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public Request UserRequest { get; set; }
        public ApplicationUser Client { get; set; }
        public string ClaimValue { get; set; }
        public bool IsRider { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Currency)]
            [Display(Name = "Delivery Credit")]
            public double DeliveryCredit { get; set; } // TODO: Culture
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await GetClientAsync(id);

            // get request instance
            UserRequest = await (from r in _context.Requests
                                 where r.Client == client
                                 select r).FirstOrDefaultAsync();

            var appUserFk = client.ApplicationUserFk;

            Client = await GetUserAsync(appUserFk);
            await GetClaimRoleAsync(Client);

            IsRider = UserRequest.Role.Equals(RoleName.Rider);

            if (UserRequest == null)
            {
                return NotFound();
            }

            return Page();
        }

        /// <summary>
        /// Get application user instance from id (Client.ApplicationUserFk)
        /// </summary>
        /// <param name="id">Identifier of ApplicationUser(IdentityUser)</param>
        /// <returns>ApplicationUser instance</returns>
        private async Task<ApplicationUser> GetUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user;
        }

        /// <summary>
        /// Get the RoleName given the ApplicationUser
        /// </summary>
        /// <param name="user">ApplicationUser</param>
        /// <returns></returns>
        private async Task GetClaimRoleAsync(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);

            var claim = claims.First(u => u.Type == ClaimName.Role);

            ClaimValue = claim.Value;
        }

        public async Task<IActionResult> OnPostAcceptBtnAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);

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
                _context.Restaurateurs.Add(new Domain.Entities.Restaurateur
                {
                    Client = UserRequest.Client,
                });
            }

            // TODO push notification to client

            await _context.SaveChangesAsync();
            return RedirectToPage("/Admin/Requests");
        }

        public async Task<IActionResult> OnPostRejectBtnAsync()
        {
            UserRequest.Status = RequestStatus.Rejected;
            Input.DeliveryCredit = 0.00;

            // update request table
            _context.Requests.Attach(UserRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                _logger.LogError($"Could not update entry of user request {UserRequest.Id} - {e.Message}");
            }

            // TODO push notification to client
            return RedirectToPage("/Admin/Requests");
        }

        /// <summary>
        /// Get the Client instance given the identifier (Client.Id)
        /// </summary>
        /// <param name="id">Identifier of the client</param>
        /// <returns>Client instance</returns>
        private async Task<Client> GetClientAsync(int? id)
        {
            try
            {
                if (id != null)
                {
                    return await (from c in _context.Clients
                        where c.Id == id
                        select c).FirstOrDefaultAsync();
                }

                throw new InvalidOperationException();
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Unable to find client with id:{id} - {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// Get the Client instance given the Application user instance
        /// </summary>
        /// <param name="id">Application user instance</param>
        /// <returns>Client instance</returns>
        private async Task<Client> GetClientAsync(ApplicationUser user)
        {
            return await (from c in _context.Clients
                where c.ApplicationUserFk == user.Id
                select c).FirstOrDefaultAsync();
        }
    }
}
