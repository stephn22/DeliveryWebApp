using DeliveryWebApp.Application.Baskets.Commands.UpdateBasket;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Products.Queries.GetProducts;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.WebUI.Pages.CustomerPages
{
    [Authorize(Policy = PolicyName.IsCustomer)]
    public class RestaurantDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RestaurantDetailModel> _logger;

        public RestaurantDetailModel(ApplicationDbContext context, IMediator mediator, UserManager<ApplicationUser> userManager, ILogger<RestaurantDetailModel> logger)
        {
            _context = context;
            _mediator = mediator;
            _userManager = userManager;
            _logger = logger;
        }

        public Restaurant Restaurant { get; set; }
        public List<Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Restaurant = await _context.Restaurants.FindAsync(id);
            Products = await _mediator.Send(new GetProductsQuery
            {
                RestaurantId = Restaurant.Id
            });

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var customer = await _context.Customers.Where(c => c.ApplicationUserFk == user.Id).FirstAsync();
            var product = await _context.Products.FindAsync(id);


            await _mediator.Send(new UpdateBasketCommand
            {
                CustomerId = customer.Id,
                Product = product
            });

            _logger.LogInformation($"Added product with id {product.Id} to basket of user {user.Id}");

            return Page();
        }
    }
}
