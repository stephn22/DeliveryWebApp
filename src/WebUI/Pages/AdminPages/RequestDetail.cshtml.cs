using System.ComponentModel;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Customers.Extensions;
using DeliveryWebApp.Application.Requests.Commands.UpdateRequest;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Application.Riders.Commands.CreateRider;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.AdminPages
{
    [Authorize(Roles = RoleName.Admin)]
    public class RequestDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RequestDetailModel> _logger;
        private readonly IMediator _mediator;

        public RequestDetailModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<RequestDetailModel> logger, IMediator mediator)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
        }

        public Request UserRequest { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ClaimValue { get; set; }
        public bool IsRider { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Currency, ErrorMessage = "Value isn't a price")]
            [DisplayName("Delivery Credit")]
            [DisplayFormat(DataFormatString = "{0:C}")]
            public decimal DeliveryCredit { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // get client by request id
            var client = await _context.GetCustomerByRequestIdAsync(id);

            // get request instance
            UserRequest = await _context.Requests.FirstAsync(r => r.Id == id);

            var appUserFk = client.ApplicationUserFk;

            ApplicationUser = await _userManager.FindByIdAsync(appUserFk);
            ClaimValue = await _userManager.GetRoleAsync(ApplicationUser);

            IsRider = UserRequest.Role.Equals(RoleName.Rider);

            if (UserRequest == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAcceptAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var customer = await _context.GetCustomerByRequestIdAsync(id);
            var appUserFk = customer.ApplicationUserFk;

            ApplicationUser = await _userManager.FindByIdAsync(appUserFk);

            var oldClaim = await _userManager.GetClaimByTypeAsync(ApplicationUser, ClaimName.Role);

            UserRequest = await _context.Requests.FirstAsync(r => r.Id == id);

            IsRider = UserRequest.Role.Equals(RoleName.Rider);

            // update tables
            if (IsRider)
            {
                var entityId = await _mediator.Send(new CreateRiderCommand()
                {
                    Customer = customer,
                    DeliveryCredit = Input.DeliveryCredit
                });

                _logger.LogInformation($"Created new rider with id: {entityId}");

                // change claim
                await _userManager.ReplaceClaimAsync(ApplicationUser, oldClaim,
                    new Claim(ClaimName.Role, RoleName.Rider));
            }
            else
            {
                var entityId = await _mediator.Send(new CreateRestaurateurCommand()
                {
                    Customer = customer,

                });

                _logger.LogInformation($"Created resturateur with id: {entityId}");

                await _userManager.ReplaceClaimAsync(ApplicationUser, oldClaim,
                    new Claim(ClaimName.Role, RoleName.Restaurateur));
            }

            await _mediator.Send(new UpdateRequestCommand
            {
                Id = UserRequest.Id,
                Status = RequestStatus.Accepted
            });

            return RedirectToPage("/AdminPages/Requests");
        }

        public async Task<IActionResult> OnPostRejectAsync(int? id)
        {
            UserRequest = await _context.Requests.FirstAsync(r => r.Id == id);
            Input.DeliveryCredit = 0.00M;

            await _mediator.Send(new UpdateRequestCommand
            {
                Id = UserRequest.Id,
                Status = RequestStatus.Rejected
            });

            return RedirectToPage("/AdminPages/Requests");
        }
    }
}
