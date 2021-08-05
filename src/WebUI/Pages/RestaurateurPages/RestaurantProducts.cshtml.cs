using System.Collections.Generic;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Products.Queries.GetProducts;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Roles = RoleName.Restaurateur)]
    public class RestaurantProductsModel : PageModel
    {
        private readonly IMediator _mediator;

        public RestaurantProductsModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public int Id { get; set; }

        public List<Product> Products { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound($"Could not find restaurant with this id");
            }

            Id = (int)id;

            Products = await _mediator.Send(new GetProductsQuery
            {
                RestaurateurId = Id
            });

            return Page();
        }
    }
}
