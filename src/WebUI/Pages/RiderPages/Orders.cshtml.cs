using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Orders.Queries.GetOrders;
using DeliveryWebApp.Domain.Constants;

namespace DeliveryWebApp.WebUI.Pages.RiderPages
{
    [Authorize(Policy = PolicyName.IsRider)]
    public class OrdersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public OrdersModel(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public List<Order> Orders { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var orders = await _mediator.Send(new GetOrdersQuery());
            Orders = orders.Where(o => o.Status == OrderStatus.New).ToList();

            return Page();
        }
    }
}
