using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Restaurateurs.Extensions;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Policy = PolicyName.IsRestaurateur)]
    public class ProductDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductDetailModel(ApplicationDbContext context, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mediator = mediator;
            _userManager = userManager;
        }

        public Product Product { get; set; }
        public Restaurateur Restaurateur { get; set; }

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
                return NotFound("Unable to load product with this ID.");
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(id, user);

            return Page();
        }

        private async Task LoadAsync(int? id, ApplicationUser user)
        {
            Product = await _context.Products.FindAsync(id);
            Restaurateur = await _context.GetRestaurateurByApplicationUserFkAsync(user.Id);
        }

        public async Task<IActionResult> OnPost(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(id, user);

            byte[] bytes = null;

            if (Input.Image != null)
            {
                await using var fileStream = Input.Image.OpenReadStream();
                await using var memoryStream = new MemoryStream();

                await fileStream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }

            await _mediator.Send(new UpdateProductCommand
            {
                Id = Product.Id,
                Name = Input.Name,
                Category = Input.Category,
                Discount = Input.Discount,
                Image = bytes,
                Price = Input.Price <= 0.00M ? Product.Price : Input.Price,
                Quantity = Input.Quantity
            });

            return RedirectToPage(id);
        }
    }
}
