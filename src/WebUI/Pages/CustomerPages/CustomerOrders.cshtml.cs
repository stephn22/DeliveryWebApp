using DeliveryWebApp.Application.Common.Models;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Orders.Extensions;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.CustomerPages
{
    [Authorize(Policy = PolicyName.IsCustomer)]
    public class CustomerOrdersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CustomerOrdersModel> _logger;
        private readonly IConfiguration _configuration;

        public CustomerOrdersModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<CustomerOrdersModel> logger, IMediator mediator, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
        }

        public Customer Customer { get; set; }
        public PaginatedList<Order> Orders { get; set; }
        public List<Product> Products { get; set; }
        public List<Restaurateur> Restaurateurs { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user, pageIndex);

            if (Customer == null || Orders == null)
            {
                return NotFound("Unable to load entities");
            }

            return Page();
        }

        private async Task LoadAsync(ApplicationUser user, int? pageIndex)
        {
            try
            {
                Customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Customer not found: {e.Message}");
                Customer = null;
                return;
            }

            Restaurateurs = new List<Restaurateur>();

            var orders = _context.Orders.Where(o => o.CustomerId == Customer.Id);

            var pageSize = _configuration.GetValue("PageSize", 5);
            Orders = await PaginatedList<Order>.CreateAsync(orders, pageIndex ?? 1, pageSize);

            if (Orders != null)
            {
                foreach (var order in Orders)
                {
                    var r = await order.GetRestaurateurAsync(_context);

                    if (r != null)
                    {
                        Restaurateurs.Add(r);
                    }
                }
            }
        }
    }
}
