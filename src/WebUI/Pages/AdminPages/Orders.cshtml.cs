using DeliveryWebApp.Application.Common.Models;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.AdminPages
{
    [Authorize(Roles = RoleName.Admin)]
    public class OrdersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public OrdersModel(ApplicationDbContext context, IMediator mediator, IConfiguration configuration)
        {
            _context = context;
            _mediator = mediator;
            _configuration = configuration;
        }

        public PaginatedList<Order> Orders { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            var orders = _context.Orders.Select(o => o);

            var pageSize = _configuration.GetValue("PageSize", 5);
            Orders = await PaginatedList<Order>.CreateAsync(orders, pageIndex ?? 1, pageSize);

            return Page();
        }

    }
}
