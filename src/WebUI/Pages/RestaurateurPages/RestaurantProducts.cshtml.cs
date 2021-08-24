using System;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Common.Security;
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
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Roles = RoleName.Restaurateur)]
    public class RestaurantProductsModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RestaurantProductsModel> _logger;

        public RestaurantProductsModel(IMediator mediator, IApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<RestaurantProductsModel> logger)
        {
            _mediator = mediator;
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public Restaurateur Restaurateur { get; set; }
        public IList<Product> Products { get; set; }

        // filtering (products table)
        public string CurrentFilter { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, string searchString)
        {
            if (id == null)
            {
                return NotFound("Unable to find a food vendor with that id");
            }

            Restaurateur = await _context.Restaurateurs.FindAsync(id);
            var products = _context.Products.Where(p => p.RestaurateurId == id);


            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.Contains(searchString));
            }

            Products = await products.AsNoTracking().ToListAsync();

            return Page();
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
