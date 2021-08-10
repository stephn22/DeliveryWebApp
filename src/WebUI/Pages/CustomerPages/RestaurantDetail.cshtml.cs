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
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Baskets.Queries;

namespace DeliveryWebApp.WebUI.Pages.CustomerPages
{
    [Authorize(Policy = PolicyName.IsCustomer)]
    public class RestaurantDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RestaurantDetailModel> _logger;

        public RestaurantDetailModel(ApplicationDbContext context, IMediator mediator,
            UserManager<ApplicationUser> userManager, ILogger<RestaurantDetailModel> logger)
        {
            _context = context;
            _mediator = mediator;
            _userManager = userManager;
            _logger = logger;
        }

        public Customer Customer { get; set; }
        public Basket Basket { get; set; }
        public Restaurateur Restaurateur { get; set; }
        public List<Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            await LoadAsync(user, id);

            if (Basket == null || Restaurateur == null)
            {
                return NotFound();
            }
            
            return Page();
        }

        private async Task LoadAsync(ApplicationUser user, int? id)
        {
            Customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);

            Basket = await _mediator.Send(new GetBasketQuery
            {
                Customer = Customer
            });

            Restaurateur = await _context.Restaurateurs.FindAsync(id);
            Products = await _mediator.Send(new GetProductsQuery
            {
                RestaurateurId = Restaurateur.Id
            });
        }

        public async Task<IActionResult> OnPostAddToCartAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var customer = await _context.Customers.Where(c => c.ApplicationUserFk == user.Id).FirstAsync();
            var product = await _context.Products.FindAsync(id);


            await _mediator.Send(new UpdateBasketCommand
            {
                Basket = Basket,
                Product = product,
                // TODO: quantity
            });

            _logger.LogInformation($"Added product with id {product.Id} to basket of user {user.Id}");

            return Page();
        }
    }
}