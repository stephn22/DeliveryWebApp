using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Infrastructure.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Roles = RoleName.Restaurateur)]
    public class RestaurantProductsModel : PageModel
    {
        public int Id { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound($"Could not find restaurant with id {id}");
            }

            Id = (int)id;

            return Page();
        }
    }
}
