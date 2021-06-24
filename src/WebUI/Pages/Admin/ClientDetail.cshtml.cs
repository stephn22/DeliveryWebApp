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
using DeliveryWebApp.Application.Clients.Queries.GetClients;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        [BindProperty]
        public IEnumerable<SelectListItem> Roles => new[]
        {
            new SelectListItem {Text = RoleName.Default, Value = RoleName.Default},
            new SelectListItem {Text = RoleName.Rider, Value = RoleName.Rider},
            new SelectListItem {Text = RoleName.Restaurateur, Value = RoleName.Restaurateur}
        };


        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Role { get; set; }

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

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByIdAsync(Client.ApplicationUserFk);
            var currentRole = await _userManager.GetRoleAsync(user);

            if (Input.Role != currentRole)
            {
                // get old claim instance
                var oldClaim = (from claim in await _userManager.GetClaimsAsync(user)
                                      where claim.Type == ClaimName.Role
                                      select claim).First();

                // replace the old claim with new claim
                await _userManager.ReplaceClaimAsync(user, oldClaim, new Claim(ClaimName.Role, Input.Role));


                //update tables
                switch (Input.Role)
                {
                    case RoleName.Rider:
                        _context.Riders.Add(new Rider
                        {
                            Client = Client,
                            DeliveryCredit = Input.DeliveryCredit,
                            OpenOrders = null
                        });
                        break;

                    case RoleName.Restaurateur:
                        _context.Restaurateurs.Add(new Domain.Entities.Restaurateur
                        {
                            Client = Client,
                            Restaurant = null,
                            Reviews = null
                        });
                        break;
                }
            }

            return RedirectToPage("/Admin/UserList");
        }
    }
}