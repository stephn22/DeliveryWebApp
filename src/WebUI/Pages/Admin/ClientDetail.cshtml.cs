using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Clients.Extensions;
using DeliveryWebApp.Application.Clients.Queries.GetClients;
using DeliveryWebApp.Application.Restaurateurs.Extensions;
using DeliveryWebApp.Application.Riders.Extensions;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.WebUI.Pages.Admin
{
    [Authorize(Roles = RoleName.Admin)]
    public class ClientDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClientDetailModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public ClientDetailModel(ApplicationDbContext context, ILogger<ClientDetailModel> logger,
            UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _mediator = mediator;
        }

        public Client Client { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public string CurrentRole { get; set; }
        public string SelectedTab { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {

            [Required]
            [DataType(DataType.Currency)]
            public double DeliveryCredit { get; set; }
        }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // get client by id
            Client = await _context.GetClientByIdAsync(id);

            var user = await _userManager.FindByIdAsync(Client.ApplicationUserFk);

            FName = await user.GetFNameAsync(_userManager);
            LName = await user.GetLNameAsync(_userManager);
            Email = await _userManager.GetEmailAsync(user);

            CurrentRole = await _userManager.GetRoleAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostToClientAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Client = await _context.GetClientByIdAsync(id);

            var user = await _userManager.FindByIdAsync(Client.ApplicationUserFk);
            CurrentRole = await _userManager.GetRoleAsync(user);

            if (CurrentRole != RoleName.Default)
            {
                // update claim
                var oldClaim = await _userManager.GetClaimByTypeAsync(user, ClaimName.Role);
                await _userManager.ReplaceClaimAsync(user, oldClaim, new Claim(ClaimName.Role, RoleName.Default));

                // update tables TODO: check if cascade
                switch (CurrentRole)
                {
                    case RoleName.Rider: // remove from table if rider
                        var rider = await _context.GetRiderByClientIdAsync(id);

                        _context.Riders.Remove(rider);
                        break;

                    case RoleName.Restaurateur: // remove from table if restaurateur
                        var restaurateur = await _context.GetRestaurateurByClientIdAsync(id);

                        _context.Restaurateurs.Remove(restaurateur);
                        break;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/UserList");
        }

        public async Task<IActionResult> OnPostToRiderAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Client = await _context.GetClientByIdAsync(id);

            var user = await _userManager.FindByIdAsync(Client.ApplicationUserFk);
            CurrentRole = await _userManager.GetRoleAsync(user);

            if (CurrentRole != RoleName.Rider)
            {
                var oldClaim = await _userManager.GetClaimByTypeAsync(user, ClaimName.Role);

                await _userManager.ReplaceClaimAsync(user, oldClaim, new Claim(ClaimName.Role, RoleName.Rider));

                // update table

                _context.Riders.Add(new Rider
                {
                    Client = Client,
                    DeliveryCredit = Input.DeliveryCredit,
                    OpenOrders = null
                });
            }
            else // update only delivery credit
            {
                var rider = await _context.GetRiderByClientIdAsync(id);

                rider.DeliveryCredit = Input.DeliveryCredit;
                _context.Riders.Update(rider);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/UserList");
        }

        public async Task<IActionResult> OnPostToRestaurateurAsync(int? id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Client = await _context.GetClientByIdAsync(id);

            var user = await _userManager.FindByIdAsync(Client.ApplicationUserFk);
            CurrentRole = await _userManager.GetRoleAsync(user);

            if (CurrentRole != RoleName.Restaurateur)
            {
                var oldClaim = await _userManager.GetClaimByTypeAsync(user, ClaimName.Role);
                await _userManager.ReplaceClaimAsync(user, oldClaim, new Claim(ClaimName.Role, RoleName.Rider));

                // update tables

                _context.Restaurateurs.Add(new Domain.Entities.Restaurateur
                {
                    Client = Client,
                    Restaurant = null,
                    Reviews = null
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Admin/UserList");
        }
    }
}