using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                return NotFound();
            }

            Id = (int)id;

            return Page();
        }
    }
}
