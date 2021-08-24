using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Orders.Extensions;
using DeliveryWebApp.Application.Orders.Queries.GetOrders;
using DeliveryWebApp.Domain.Entities;
using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Models;
using Microsoft.Extensions.Configuration;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Roles = RoleName.Restaurateur)]
    public class RestaurantOrdersModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public RestaurantOrdersModel(IMediator mediator, IApplicationDbContext context, IConfiguration configuration)
        {
            _mediator = mediator;
            _context = context;
            _configuration = configuration;
        }

        public Restaurateur Restaurateur { get; set; }
        public PaginatedList<Order> Orders { get; set; }
        public List<Product> Products { get; set; }
        public List<Address> Addresses { get; set; }

        public async Task<IActionResult> OnGet(int? id, int? pageIndex)
        {
            if (id == null)
            {
                return NotFound($"Could not find restaurant with this id");
            }

            await LoadAsync((int)id, pageIndex);

            if (Restaurateur == null)
            {
                return NotFound("Could not load entities");
            }

            return Page();
        }

        private async Task LoadAsync(int id, int? pageIndex)
        {
            Restaurateur = await _context.Restaurateurs.FindAsync(id);

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
