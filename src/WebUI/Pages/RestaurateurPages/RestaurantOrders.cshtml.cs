using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Orders.Queries.GetOrders;
using DeliveryWebApp.Application.Restaurants.Queries.GetRestaurants;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    public class RestaurantOrdersModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;

        public RestaurantOrdersModel(IMediator mediator, IApplicationDbContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        public int Id { get; set; }

        public Restaurant Restaurant { get; set; }

        public List<Order> Orders { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound($"Could not find restaurant with this id");
            }

            Id = (int)id;

            Restaurant = await _context.Restaurants.FindAsync(Id);

            Orders = await _mediator.Send(new GetOrdersQuery
            {
                RestaurantId = Id
            });

            return Page();
        }
    }
}
