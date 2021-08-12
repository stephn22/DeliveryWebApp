using System;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Products.Commands.DeleteProduct;
using DeliveryWebApp.Application.Products.Queries.GetProducts;
using DeliveryWebApp.Application.Restaurateurs.Extensions;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Infrastructure.Security;
using EllipticCurve;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Roles = RoleName.Restaurateur)]
    public class RestaurantProductsModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public RestaurantProductsModel(IMediator mediator, IApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger logger)
        {
            _mediator = mediator;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public Restaurateur Restaurateur { get; set; }
        public IList<Product> Products { get; set; }

        // sort orders (products table)
        public string NameSort { get; set; }
        public string QuantitySort { get; set; }
        public string PriceSort { get; set; }
        public string DiscountSort { get; set; }
        public string CategorySort { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, string sortOrder)
        {
            if (id == null)
            {
                return NotFound($"Unable to find a food vendor with that id");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return BadRequest();
            }

            await LoadAsync(user);
            await ProductsSorting(sortOrder, Restaurateur.Id);

            return Page();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            Restaurateur = await _context.GetRestaurateurByApplicationUserFkAsync(user.Id);

            if (Restaurateur != null)
            {
                Products = await _mediator.Send(new GetProductsQuery
                {
                    RestaurateurId = Restaurateur.Id
                });
            }
        }

        /// <summary>
        /// Sort products in table
        /// </summary>
        /// <param name="sortOrder">order which table has to be sorted</param>
        /// <param name="restaurateurId">id of food vendor</param>
        /// <returns></returns>
        private async Task ProductsSorting(string sortOrder, int restaurateurId)
        {
            NameSort = string.IsNullOrEmpty(sortOrder) ? ProductSortOrders.NameDesc : "";
            QuantitySort = string.IsNullOrEmpty(sortOrder) ? ProductSortOrders.QuantityDesc : "";
            PriceSort = string.IsNullOrEmpty(sortOrder) ? ProductSortOrders.PriceDesc : "";
            DiscountSort = string.IsNullOrEmpty(sortOrder) ? ProductSortOrders.DiscountDesc : "";
            CategorySort = string.IsNullOrEmpty(sortOrder) ? ProductSortOrders.CategoryDesc : "";

            var products = _context.Products.Where(p => p.RestaurateurId == restaurateurId);

            products = sortOrder switch
            {
                ProductSortOrders.NameDesc => products.OrderByDescending(p => p.Name),
                ProductSortOrders.QuantityDesc => products.OrderByDescending(p => p.Quantity),
                ProductSortOrders.PriceDesc => products.OrderByDescending(p => p.Price),
                ProductSortOrders.DiscountDesc => products.OrderByDescending(p => p.Discount),
                ProductSortOrders.CategoryDesc => products.OrderByDescending(p => p.Category),
                _ => products.OrderByDescending(p => p.Id)
            };

            Products = await products.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            try
            {
                await _mediator.Send(new DeleteProductCommand
                {
                    Id = product.Id
                });
            }
            catch (NotFoundException e)
            {
                _logger.LogWarning($"{e.Message}");
                return RedirectToPage();
            }

            _logger.LogInformation($"Deleted product with id '{product.Id}");

            return RedirectToPage();
        }
    }
}
