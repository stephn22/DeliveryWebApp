using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Restaurateurs.Extensions;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Policy = PolicyName.IsRestaurateur)]
    public class AddProductModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ILogger<AddProductModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<AddProductModel> _localizer;

        public AddProductModel(IMediator mediator, ILogger<AddProductModel> logger,
            UserManager<ApplicationUser> userManager, ApplicationDbContext context,
            IStringLocalizer<AddProductModel> localizer)
        {
            _mediator = mediator;
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _localizer = localizer;
        }

        [BindProperty]
        public IEnumerable<SelectListItem> Categories => new[]
        {
            new SelectListItem { Text = _localizer[ProductCategory.Unassigned], Value = ProductCategory.Unassigned },
            new SelectListItem { Text = ProductCategory.Hamburger, Value = ProductCategory.Hamburger },
            new SelectListItem { Text = ProductCategory.Pizza, Value = ProductCategory.Pizza },
            new SelectListItem { Text = ProductCategory.Sushi, Value = ProductCategory.Sushi },
            new SelectListItem { Text = ProductCategory.Dessert, Value = ProductCategory.Dessert },
            new SelectListItem { Text = ProductCategory.Vegan, Value = ProductCategory.Vegan },
            new SelectListItem { Text = _localizer[ProductCategory.Chicken], Value = ProductCategory.Chicken },
            new SelectListItem { Text = _localizer[ProductCategory.Fish], Value = ProductCategory.Fish },
            new SelectListItem { Text = ProductCategory.Snacks, Value = ProductCategory.Snacks },
        };

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [StringLength(60,
                ErrorMessage = "The {0} must be at least {1} characters long and at max {2} characters long.",
                MinimumLength = 3)]
            public string Name { get; set; }

            [DataType(DataType.Upload)]
            [DisplayName("Product Image")]
            public IFormFile Image { get; set; }

            [Required]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
            [DataType(DataType.Currency, ErrorMessage = "Value isn't a price")]
            [DisplayFormat(DataFormatString = "{0:C}")]
            public decimal Price { get; set; }

            [RegularExpression("^[0-9][0-9]?$|^100$", ErrorMessage = "The {0} must be digits only from 0 to 100.")]
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

            byte[] bytes = null;

            if (Input.Image != null)
            {
                await using var fileStream = Input.Image.OpenReadStream();
                await using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }

            await _mediator.Send(new CreateProductCommand
            {
                Image = bytes,
                Name = Input.Name,
                Category = Input.Category,
                Discount = Input.Discount,
                Price = Input.Price,
                Quantity = Input.Quantity,
                RestaurateurId = restaurateur.Id
            });

            return Redirect($"/RestaurateurPages/RestaurantProducts/{restaurateur.Id}");
        }
    }
}