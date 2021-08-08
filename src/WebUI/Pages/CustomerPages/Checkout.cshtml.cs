using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Addresses.Queries.GetAddresses;
using DeliveryWebApp.Application.Baskets.Queries;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.WebUI.Pages.CustomerPages
{
    [Authorize(Policy = PolicyName.IsCustomer)]
    public class CheckoutModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CheckoutModel> _logger;
        private readonly IMediator _mediator;

        public CheckoutModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<CheckoutModel> logger, IMediator mediator)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
        }

        public Basket Basket { get; set; }
        public Customer Customer { get; set; }
        public List<Address> CustomerAddresses { get; set; }
        public List<Product> Products { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            Customer = await _context.Customers.Where(c => c.ApplicationUserFk == user.Id).FirstAsync();

            CustomerAddresses = await _mediator.Send(new GetAddressesQuery
            {
                CustomerId = Customer.Id
            });

            Basket = await _mediator.Send(new GetBasketQuery
            {
                CustomerId = Customer.Id
            });

            // TODO: BasketProducts

            return Page();
        }
    }
}
