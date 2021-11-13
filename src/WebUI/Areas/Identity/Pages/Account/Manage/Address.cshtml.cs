using DeliveryWebApp.Application.Addresses.Commands.DeleteAddress;
using DeliveryWebApp.Application.Addresses.Queries.GetAddresses;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Areas.Identity.Pages.Account.Manage
{
    [Authorize(Policy = PolicyName.IsCustomer)]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    public partial class AddressModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AddressModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<AddressModel> _localizer;

        public AddressModel(UserManager<ApplicationUser> userManager, ILogger<AddressModel> logger,
            ApplicationDbContext context, IMediator mediator, IStringLocalizer<AddressModel> localizer)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
            _mediator = mediator;
            _localizer = localizer;
        }

        [TempData] public string StatusMessage { get; set; }

        public List<Address> Addresses { get; set; }

        public Customer Customer { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);

            StatusMessage = "";

            await LoadAddressesAsync(user);
            return Page();
        }

        private async Task LoadAddressesAsync(ApplicationUser user)
        {
            Customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);

            Addresses = await _mediator.Send(new GetAddressesQuery
            {
                CustomerId = Customer.Id
            });
        }
    }
}