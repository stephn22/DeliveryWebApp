using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Requests.Commands.CreateRequest;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Policy = PolicyName.IsCustomer)]
    public class RequestModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RequestModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public RequestModel(UserManager<ApplicationUser> userManager, ILogger<RequestModel> logger,
            ApplicationDbContext context, SignInManager<ApplicationUser> signInManager, IMediator mediator)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
            _mediator = mediator;
            _signInManager = signInManager;
        }

        public bool HasRequest { get; set; }

        public Request UserRequest { get; set; }

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
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadRequestAsync(user);
            return Page();
        }

        private async Task LoadRequestAsync(ApplicationUser user)
        {
            try
            {
                UserRequest = await (_context.Requests.FirstAsync(r => r.Customer.ApplicationUserFk == user.Id));
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
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadRequestAsync(user);
                return Page();
            }

            var customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);

            var requestId = await _mediator.Send(new CreateRequestCommand
            {
                CustomerId = customer.Id,
                Role = Input.Role,
                Status = RequestStatus.Idle
            });

            await _signInManager.RefreshSignInAsync(user);

            _logger.LogInformation($"User posted their request successfully with id: {requestId}");

            return RedirectToPage();
        }
    }
}