using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.AdminPages
{
    [Authorize(Roles = RoleName.Admin)]
    public class OrderDetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Order Order { get; set; }
        public Restaurateur Restaurateur { get; set; }
        public Rider Rider { get; set; }
        public Address Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order = await _context.Orders.FindAsync(id);

            if (Order == null)
            {
                return NotFound();
            }

            Restaurateur = await _context.Restaurateurs.FindAsync(Order.RestaurateurId);

            if (Restaurateur == null)
            {
                return NotFound();
            }

            Rider = await _context.Riders.FindAsync(Order.RiderId);

            Address = await _context.Addresses.FindAsync(Order.AddressId);

            return Page();
        }
    }
}
