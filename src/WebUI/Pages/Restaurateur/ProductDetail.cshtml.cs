using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.WebUI.Pages.Restaurateur
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

        // TODO: Selectlist item for category
        [BindProperty] public IEnumerable<SelectListItem> Categories { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string Category { get; set; }

            [Required]
            [DataType(DataType.Currency)]
            public double Price { get; set; }

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
    }
}
