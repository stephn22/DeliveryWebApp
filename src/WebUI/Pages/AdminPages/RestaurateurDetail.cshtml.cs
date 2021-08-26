using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Restaurateurs.Commands.DeleteRestaurateur;
using DeliveryWebApp.Application.Riders.Commands.CreateRider;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.AdminPages
{
    [Authorize(Roles = RoleName.Admin)]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    public class RestaurateurDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RestaurateurDetailModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public RestaurateurDetailModel(ApplicationDbContext context, ILogger<RestaurateurDetailModel> logger, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }

        public Customer Customer { get; set; }
        public Restaurateur Restaurateur { get; set; }
        public ApplicationUser User { get; set; }
        public string CurrentRole { get; set; }

        [BindProperty] public InputModel Input { get; set; }


        public class InputModel
        {
            [Required]
            [DataType(DataType.Currency)]
            [DisplayName("Delivery Credit")]
            public decimal DeliveryCredit { get; set; }
        }

        private async Task LoadAsync(int id)
        {
            Restaurateur = await _context.Restaurateurs.FindAsync(id);
            Restaurateur.RestaurantAddress = await _context.Addresses.FindAsync(Restaurateur.RestaurantAddressId);

            if (Restaurateur != null)
            {
                Customer = await _context.Customers.FindAsync(Restaurateur.CustomerId);

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
                return NotFound("Unable to find restaurateur with that id");
            }

            await LoadAsync((int)id);

            if (Restaurateur == null)
            {
                return NotFound("Unable to load entities");
            }

            return Page();
        }


        public async Task<IActionResult> OnPostBlockRestaurateurAsync(int? id)
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

        public async Task<IActionResult> OnPostUnblockRestaurateurAsync(int? id)
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

            await _mediator.Send(new DeleteRestaurateurCommand
            {
                Id = Restaurateur.Id
            });

            _logger.LogInformation($"Deleted restaurateur with id '{Restaurateur.Id}");

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostToCustomerAsync(int? id)
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
            await _mediator.Send(new DeleteRestaurateurCommand
            {
                Id = Restaurateur.Id
            });

            _logger.LogInformation($"Deleted restaurateur with id {Restaurateur.Id}");

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
            await _userManager.ReplaceClaimAsync(User, oldClaim, new Claim(ClaimName.Role, RoleName.Rider));

            // update tables
            await _mediator.Send(new DeleteRestaurateurCommand
            {
                Id = Restaurateur.Id
            });

            _logger.LogInformation($"Deleted restaurateur with id {Restaurateur.Id}");

            var rider = await _mediator.Send(new CreateRiderCommand
            {
                Customer = Customer,
                DeliveryCredit = Input.DeliveryCredit
            });

            _logger.LogInformation($"Created rider with id {rider.Id}");

            return RedirectToPage("/AdminPages/UserList");
        }
    }
}
