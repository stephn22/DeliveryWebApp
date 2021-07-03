using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.WebUI.Pages.CustomerPages
{
    [Authorize(Policy = PolicyName.IsCustomer)]
    public class OrdersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OrdersModel> _logger;

        public OrdersModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<OrdersModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public Order Order { get; set; }
        public IList<Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // TODO: mediatr
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            await GetOrderAsync(user);

            return Page();
        }

        private async Task GetOrderAsync(ApplicationUser user)
        {
            Order = await (from o in _context.Orders
                         where o.Customer.ApplicationUserFk == user.Id
                         select o).FirstOrDefaultAsync();

            // Products = Order.Products;
            GetProducts();
        }

        private void GetProducts()
        {
            // TODO: mediatr
            Products = Order.Products.Select(p => new Product
            {
                Name = p.Name,
                Image = p.Image,
                Price = p.Price,
                Discount = p.Discount,
                Category = p.Category
            }).ToList();
        }
    }
}