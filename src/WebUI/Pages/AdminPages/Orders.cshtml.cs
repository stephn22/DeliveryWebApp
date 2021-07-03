using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.WebUI.Pages.AdminPages
{
    [Authorize(Roles = RoleName.Admin)]
    public class OrdersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OrdersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Order> Orders { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var orders = from o in _context.Orders
                         orderby o.Date descending
                         select o;

            Orders = await orders.ToListAsync();

            return Page();
        }
    }
}