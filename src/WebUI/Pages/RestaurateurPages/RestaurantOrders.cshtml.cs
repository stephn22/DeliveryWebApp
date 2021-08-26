using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Common.Models;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Orders.Extensions;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using IdentityServer4.Extensions;
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

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Roles = RoleName.Restaurateur)]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    public class RestaurantOrdersModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RestaurantOrdersModel> _logger;

        public RestaurantOrdersModel(IMediator mediator, IApplicationDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager, ILogger<RestaurantOrdersModel> logger)
        {
            _mediator = mediator;
            _context = context;
            _configuration = configuration;
            _userManager = userManager;
            _logger = logger;
        }

        public Restaurateur Restaurateur { get; set; }
        public PaginatedList<Order> Orders { get; set; }
        public List<Product> Products { get; set; }
        public List<Address> Addresses { get; set; }

        public async Task<IActionResult> OnGet(int? pageIndex)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(pageIndex, user);

            if (Restaurateur == null)
            {
                return NotFound("Could not load entities");
            }

            return Page();
        }

        private async Task LoadAsync(int? pageIndex, ApplicationUser user)
        {
            try
            {
                var customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);
                Restaurateur = await _context.Restaurateurs.FirstAsync(r => r.CustomerId == customer.Id);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e.Message);
                Restaurateur = null;
            }

            if (Restaurateur != null)
            {
                var orders = _context.Orders.Where(o => o.RestaurateurId == Restaurateur.Id);

                var pageSize = _configuration.GetValue("PageSize", 5);
                Orders = await PaginatedList<Order>.CreateAsync(orders, pageIndex ?? 1, pageSize);

                if (!Orders.IsNullOrEmpty())
                {
                    Addresses = new List<Address>();
                    Products = new List<Product>();

                    foreach (var order in Orders)
                    {
                        var a = await order.GetAddressAsync(_context);

                        Addresses.Add(a);
                    }
                }
            }
        }
    }
}
