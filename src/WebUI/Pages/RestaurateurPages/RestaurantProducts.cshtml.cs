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

        private async Task LoadAsync(int id, string searchString, string currentFilter, int? pageIndex)
        {
            Restaurateur = await _context.Restaurateurs.FindAsync(id);

            if (Restaurateur != null)
            {
                var products = _context.Products.Where(p => p.RestaurateurId == id);

                if (!string.IsNullOrEmpty(searchString))
                {
                    products = products.Where(p => p.Name.ToLower().Contains(searchString.ToLower()));
                }

                var pageSize = _configuration.GetValue("PageSize", 5);
                Products = await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageIndex ?? 1, pageSize);
            }
        }

        public async Task<IActionResult> OnGetAsync(int? id, string searchString, string currentFilter, int? pageIndex)
        {
            if (id == null)
            {
                return NotFound("Unable to find a food vendor with that id");
            }

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            await LoadAsync((int)id, searchString, currentFilter, pageIndex);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteProductAsync(int id, int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            await LoadAsync(id, "", "", 1);

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
