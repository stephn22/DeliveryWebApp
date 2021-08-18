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
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [BindProperty]
        public List<SelectListItem> Quantities { get; set; }

        [BindProperty]
        [Required]
        public int Quantity { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

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
            Restaurateur = await _context.Restaurateurs.FindAsync(id);

            try
            {
                Products = await _mediator.Send(new GetProductsQuery
                {
                    RestaurateurId = Restaurateur.Id
                });
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation($"Unable to find food vendor with id '{id}': {e.Message}");
                Restaurateur = null;
                return;
            }

            try
            {
                Customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogInformation($"Unable to find customer: {e.Message}");
                Customer = null;
                return;
            }

            Basket = await _mediator.Send(new GetBasketQuery
            {
                Customer = Customer
            }) // If basket does not exist (null), create a new one
                     ?? await _mediator.Send(new CreateBasketCommand
                     {
                         Customer = Customer
                     });

            _logger.LogInformation($"Created new basket with id: {Basket.Id}");
        }

        public List<SelectListItem> QuantityList(int qty)
        {
            Quantities = new List<SelectListItem>(qty);

            for (var i = 1; i <= qty; i++)
            {
                Quantities.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            return Quantities;
        }

        public async Task<IActionResult> OnPostAddToBasketAsync(int id, int productId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);

            await LoadAsync(user, id);

            if (Restaurateur == null || Customer == null)
            {
                return NotFound("Unable to find entities");
            }

            var product = Products.First(p => p.Id == productId);

            await _mediator.Send(new UpdateBasketCommand
            {
                Basket = Basket,
                Product = product,
                Quantity = Quantity,
                RestaurateurId = Restaurateur.Id
            });

            _logger.LogInformation($"Added product with id {product.Id} to the basket of the user {user.Id}");

            StatusMessage = "Added product to cart successfully";

            return RedirectToPage();
        }
    }
}
