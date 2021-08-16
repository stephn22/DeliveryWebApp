using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;
using DeliveryWebApp.Application.Baskets.Commands.UpdateBasket;
using DeliveryWebApp.Application.Baskets.Queries;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Products.Queries.GetProducts;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
        public InputModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            public int Quantity { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound("Unable to find food vendor with that id");
            }

            var user = await _userManager.GetUserAsync(User);
            await LoadAsync(user, id);

            if (Restaurateur == null)
            {
                return NotFound("Unable to find food vendor with that id");
            }

            return Page();
        }

        private async Task LoadAsync(ApplicationUser user, int? id)
        {
            try
            {
                Restaurateur = await _context.Restaurateurs.FindAsync(id);

                Products = await _mediator.Send(new GetProductsQuery
                {
                    RestaurateurId = Restaurateur.Id
                });
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation($"Unable to find food vendor with id '{id}': {e.Message}");
                Restaurateur = null;
            }

            Customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);

            try
            {
                Basket = await _mediator.Send(new GetBasketQuery
                {
                    Customer = Customer
                });
            }
            catch (InvalidOperationException)
            {
                // If basket does not exist, create a new one
                Basket = await _mediator.Send(new CreateBasketCommand
                {
                    Customer = Customer
                });

                _logger.LogInformation($"Created new basket with id: {Basket.Id}");
            }
        }

        public async Task OnPostAddToCartAsync(int id, int productId)
        {
            var user = await _userManager.GetUserAsync(User);

            await LoadAsync(user, id);

            await _context.Customers.Where(c => c.ApplicationUserFk == user.Id).FirstAsync();
            var product = Products.First(p => p.Id == productId);

            await _mediator.Send(new UpdateBasketCommand
            {
                Basket = Basket,
                Product = product,
                Quantity = Input.Quantity
            });

            _logger.LogInformation($"Added product with id {product.Id} to the basket of the user {user.Id}");

            StatusMessage = "Added product to cart successfully";
        }
    }
}
