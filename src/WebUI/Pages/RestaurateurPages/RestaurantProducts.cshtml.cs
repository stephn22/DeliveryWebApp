using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Common.Models;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Products.Commands.DeleteProduct;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Roles = RoleName.Restaurateur)]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    public class RestaurantProductsModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RestaurantProductsModel> _logger;
        private readonly IConfiguration _configuration;

        public RestaurantProductsModel(IMediator mediator, IApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<RestaurantProductsModel> logger, IConfiguration configuration)
        {
            _mediator = mediator;
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        public Restaurateur Restaurateur { get; set; }
        public PaginatedList<Product> Products { get; set; }

        // filtering (products table)
        public string CurrentFilter { get; set; }

        private async Task LoadAsync(ApplicationUser user, string searchString, string currentFilter, int? pageIndex)
        {
            try
            {
                var customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);
                Restaurateur = await _context.Restaurateurs.FirstAsync(r => r.CustomerId == customer.Id);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e.Message);
                Restaurateur = null;
            }

            if (Restaurateur != null)
            {
                var products = _context.Products.Where(p => p.RestaurateurId == Restaurateur.Id);

                if (!string.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p => p.Name.ToLower().Contains(searchString.ToLower()));
                }

                var pageSize = _configuration.GetValue("PageSize", 5);
                Products = await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
        }

        public async Task<IActionResult> OnGetAsync(string searchString, string currentFilter, int? pageIndex)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            await LoadAsync(user, searchString, currentFilter, pageIndex);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteProductAsync(int productId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(productId);

            await LoadAsync(user, "", "", 1);

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
