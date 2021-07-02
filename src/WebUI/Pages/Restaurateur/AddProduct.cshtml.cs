using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Restaurants.Commands.UpdateRestaurant;
using DeliveryWebApp.Application.Restaurants.Extensions;
using DeliveryWebApp.Application.Restaurateurs.Extensions;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.WebUI.Pages.Restaurateur
{
    [Authorize(Policy = PolicyName.IsRestaurateur)]
    public class AddProductModel : PageModel
    {
        // TODO: culture and localize
        private readonly IMediator _mediator;
        private readonly ILogger<AddProductModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AddProductModel(IMediator mediator, ILogger<AddProductModel> logger, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _mediator = mediator;
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        // TODO: Selectlist item for category
        [BindProperty] public IEnumerable<SelectListItem> Categories  { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [StringLength(60, ErrorMessage = "The {0} must be at least {1} characters long and at max {2} characters long.", MinimumLength = 3)]
            public string Name { get; set; }

            [DataType(DataType.Upload)]
            [DisplayName("Product Image")]
            public IFormFile Image { get; set; }

            [Required]
            [DataType(DataType.Currency, ErrorMessage = "Value isn't a price")]
            public double Price { get; set; }

            [Required]
            [RegularExpression("^[0-9][0-9]?$|^100$", ErrorMessage = "The {0} must be digits only from 0 to 100.")]
            [DisplayName("Discount (0 for no discount)")]
            public int Discount { get; set; }

            [DataType(DataType.Text)] public string Category { get; set; }

            [Required] public int Quantity { get; set; }
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);

            var restaurateur = await _context.GetRestaurateurByApplicationUserFkAsync(user.Id);
            var restaurant = await _context.GetRestaurantByRestaurateurId(restaurateur.Id);



            byte[] bytes;

            await using var fileStream = Input.Image.OpenReadStream();
            await using (var memoryStream = new MemoryStream())
            {
                await fileStream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }

            var product = new Product
            {
                Image = bytes,
                Name = Input.Name,
                Category = Input.Category,
                Discount = Input.Discount,
                Price = Input.Price,
                Quantity = Input.Quantity
            };

            await _mediator.Send(new UpdateRestaurantCommand
            {
                Id = restaurant.Id,
                Product = product

            });

            return RedirectToPage("/Restaurateur/RestaurantDashboard");
        }
    }
}
