using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Application.Riders.Commands.DeleteRider;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.AdminPages
{
    public class RiderDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RiderDetailModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public RiderDetailModel(ApplicationDbContext context, ILogger<RiderDetailModel> logger, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }

        public Customer Customer { get; set; }
        public Rider Rider { get; set; }
        public ApplicationUser User { get; set; }
        public string CurrentRole { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Currency)]
            public decimal DeliveryCredit { get; set; }
        }

        private async Task LoadAsync(int id)
        {
            Rider = await _context.Riders.FindAsync(id);

            if (Rider != null)
            {
                Customer = await _context.Customers.FindAsync(Rider.CustomerId);

                if (Customer != null)
                {
                    User = await _userManager.FindByIdAsync(Customer.ApplicationUserFk);

                    CurrentRole = await _userManager.GetRoleAsync(User);
                }
            }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound("Unable to find rider with that id");
            }

            await LoadAsync((int)id);

            if (Rider == null)
            {
                return NotFound("Unable to load entities");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostBlockRiderAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

            await _userManager.BlockUser(User);

            _logger.LogInformation($"Blocked user with id '{User.Id}");

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostUnblockRiderAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

            await _userManager.UnblockUser(User);

            _logger.LogInformation($"Unblocked user with id '{User.Id}");

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

            await _userManager.DeleteAsync(User);

            await _mediator.Send(new DeleteRiderCommand
            {
                Id = Rider.Id
            });

            _logger.LogInformation($"Deleted rider with id '{Rider.Id}");

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostToRiderAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

            var oldClaim = await _userManager.GetClaimByTypeAsync(User, ClaimName.Role);
            await _userManager.ReplaceClaimAsync(User, oldClaim, new Claim(ClaimName.Role, RoleName.Default));

            // update tables
            await _mediator.Send(new DeleteRiderCommand
            {
                Id = Rider.Id
            });

            _logger.LogInformation($"Deleted rider with id {Rider.Id}");

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostToRestaurateurAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

            var oldClaim = await _userManager.GetClaimByTypeAsync(User, ClaimName.Role);
            await _userManager.ReplaceClaimAsync(User, oldClaim, new Claim(ClaimName.Role, RoleName.Restaurateur));

            // update tables
            await _mediator.Send(new DeleteRiderCommand
            {
                Id = Rider.Id
            });

            _logger.LogInformation($"Deleted rider with id {Rider.Id}");

            var restaurateur = await _mediator.Send(new CreateRestaurateurCommand
            {
                Customer = Customer
            });

            _logger.LogInformation($"Created rider with id {restaurateur.Id}");

            return RedirectToPage("/AdminPages/UserList");
        }
    }
}
