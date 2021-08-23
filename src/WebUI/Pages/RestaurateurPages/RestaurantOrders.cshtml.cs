using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.OrderItems.Extensions;
using DeliveryWebApp.Application.OrderItems.Queries;
using DeliveryWebApp.Application.Orders.Extensions;
using DeliveryWebApp.Application.Orders.Queries.GetOrders;
using DeliveryWebApp.Domain.Entities;
using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Roles = RoleName.Restaurateur)]
    public class RestaurantOrdersModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public RestaurantOrdersModel(IMediator mediator, IApplicationDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public Restaurateur Restaurateur { get; set; }

        public List<Order> Orders { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Product> Products { get; set; }
        public List<Address> Addresses { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound($"Could not find restaurant with this id");
            }

            await LoadAsync((int)id);

            if (Restaurateur == null)
            {
                return NotFound("Could not load entities");
            }

            return Page();
        }

        private async Task LoadAsync(int id)
        {
            Restaurateur = await _context.Restaurateurs.FindAsync(id);

            if (Restaurateur != null)
            {
                Orders = await _mediator.Send(new GetOrdersQuery
                {
                    RestaurateurId = Restaurateur.Id
                });

                if (!Orders.IsNullOrEmpty())
                {
                    OrderItems = new List<OrderItem>();
                    Addresses = new List<Address>();

                    foreach (var order in Orders)
                    {
                        var partialList = await _mediator.Send(new GetOrderItemsQuery
                        {
                            OrderId = order.Id
                        });

                        var a = await order.GetAddressAsync(_context);

                        OrderItems.AddRange(partialList);
                        Addresses.Add(a);
                    }

                    if (!OrderItems.IsNullOrEmpty())
                    {
                        Products = new List<Product>();

                        foreach (var item in OrderItems)
                        {
                            var p = await item.GetProduct(_context);

                            if (p != null)
                            {
                                Products.Add(p);
                            }

                        }
                    }
                }
            }
        }
    }
}
