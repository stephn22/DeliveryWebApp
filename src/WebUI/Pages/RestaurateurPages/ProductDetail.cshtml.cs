using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Policy = PolicyName.IsRestaurateur)]
    public class ProductDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductDetailModel> _logger;
        private readonly IMediator _mediator;

        public ProductDetailModel(ApplicationDbContext context, ILogger<ProductDetailModel> logger, IMediator mediator)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
        }

        public Product Product { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> Categories => new[]
        {
            new SelectListItem {Text = ProductCategory.Unassigned, Value = ProductCategory.Unassigned},
            new SelectListItem {Text = ProductCategory.Chicken, Value = ProductCategory.Chicken},
            new SelectListItem {Text = ProductCategory.Dessert, Value = ProductCategory.Dessert},
            new SelectListItem {Text = ProductCategory.Sushi, Value = ProductCategory.Sushi},
            new SelectListItem {Text = ProductCategory.Vegan, Value = ProductCategory.Vegan},
            new SelectListItem {Text = ProductCategory.Hamburger, Value = ProductCategory.Hamburger},
            new SelectListItem {Text = ProductCategory.Fish, Value = ProductCategory.Fish},
            new SelectListItem {Text = ProductCategory.Drink, Value = ProductCategory.Drink},
            new SelectListItem {Text = ProductCategory.Pizza, Value = ProductCategory.Pizza}
        };

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Upload)]
            public IFormFile Image { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string Category { get; set; }

            [Required]
            public decimal Price { get; set; }

            [Required]
            [RegularExpression("^[0-9][0-9]?$|^100$", ErrorMessage = "The {0} must be digits only from 0 to 100.")]
            [DisplayName("Discount (0 for no discount)")]
            public int Discount { get; set; }

            [Required] public int Quantity { get; set; }
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound($"Unable to load product with this ID.");
            }

            Product = await _context.Products.FindAsync(id);

            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            byte[] bytes = null;

            if (Input.Image != null)
            {
                await using var fileStream = Input.Image.OpenReadStream();
                await using var memoryStream = new MemoryStream();

                await fileStream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }

            Product.Name = Input.Name;
            Product.Category = Input.Category;
            Product.Discount = Input.Discount;
            Product.Image = bytes;
            Product.Price = Input.Price;
            Product.Quantity = Input.Quantity;

            await _mediator.Send(new UpdateProductCommand
            {
                Product = Product
            });

            return RedirectToPage(routeValues: id);
        }
    }
}
