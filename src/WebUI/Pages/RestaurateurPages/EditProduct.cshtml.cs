using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    public class EditProductModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly ILogger<EditProductModel> _logger;
        private readonly IStringLocalizer<EditProductModel> _localizer;
        private readonly IMapper _mapper;

        public EditProductModel(ApplicationDbContext context, IMediator mediator, ILogger<EditProductModel> logger, IStringLocalizer<EditProductModel> localizer, IMapper mapper)
        {
            _context = context;
            _mediator = mediator;
            _logger = logger;
            _localizer = localizer;
            _mapper = mapper;
        }
        public Product Product { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [DataType(DataType.Upload)] public IFormFile Logo { get; set; }
            [Required] [DataType(DataType.Text)] public string Name { get; set; }

            [Required] [DataType(DataType.Text)] public string Category { get; set; }

            [Required]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
            [DataType(DataType.Currency, ErrorMessage = "Value isn't a price")]
            public decimal Price { get; set; }

            [Required]
            [RegularExpression("^[0-9][0-9]?$|^100$", ErrorMessage = "The {0} must be digits only from 0 to 100.")]
            [DisplayName("Discount")]
            public int Discount { get; set; }

            [Required] public int Quantity { get; set; }
        }

        [BindProperty]
        public IEnumerable<SelectListItem> Categories => new[]
        {
            new SelectListItem { Text = _localizer[ProductCategory.Unassigned], Value = ProductCategory.Unassigned },
            new SelectListItem { Text = ProductCategory.Hamburger, Value = ProductCategory.Hamburger },
            new SelectListItem { Text = ProductCategory.Pizza, Value = ProductCategory.Pizza },
            new SelectListItem { Text = ProductCategory.Sushi, Value = ProductCategory.Sushi },
            new SelectListItem { Text = _localizer[ProductCategory.Drink], Value = ProductCategory.Drink },
            new SelectListItem { Text = ProductCategory.Dessert, Value = ProductCategory.Dessert },
            new SelectListItem { Text = ProductCategory.Vegan, Value = ProductCategory.Vegan },
            new SelectListItem { Text = _localizer[ProductCategory.Chicken], Value =  ProductCategory.Chicken},
            new SelectListItem { Text = _localizer[ProductCategory.Fish], Value =  ProductCategory.Fish},
            new SelectListItem { Text = ProductCategory.Snacks, Value = ProductCategory.Snacks },
        };

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        private async Task LoadAsync(int id)
        {
            Product = await _context.Products.FindAsync(id);

            if (Product != null)
            {
                foreach (var item in Categories)
                {
                    item.Selected = item.Value == Product.Category;
                }
            }
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            Product = await _context.Products.FindAsync(id);

            byte[] bytes = null;

            if (Input.Logo != null)
            {
                await using var fileStream = Input.Logo.OpenReadStream();
                await using var memoryStream = new MemoryStream();

                await fileStream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }

            try
            {
                await _mediator.Send(new UpdateProductCommand
                {
                    Id = Product.Id,
                    Image = bytes,
                    Category = Input.Category,
                    Discount = Input.Discount,
                    Name = Input.Name,
                    Price = Input.Price, /*decimal.Parse(Input.Price.ToString(CultureInfo.InvariantCulture)),*/
                    Quantity = Input.Quantity
                });

                _logger.LogInformation($"Updated product with id {Product.Id}");
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e.Message);
                return NotFound();
            }

            return Redirect($"/RestaurateurPages/ProductDetail/{Product.Id}");
        }
    }
}
