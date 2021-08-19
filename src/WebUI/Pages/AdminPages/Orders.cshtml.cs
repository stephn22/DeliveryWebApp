using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Orders.Queries.GetOrders;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.AdminPages
{
    [Authorize(Roles = RoleName.Admin)]
    public class OrdersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public OrdersModel(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public IList<Order> Orders { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadAsync();

            return Page();
        }

        private async Task LoadAsync()
        {
            Orders = await _mediator.Send(new GetOrdersQuery());
        }
    }
}
