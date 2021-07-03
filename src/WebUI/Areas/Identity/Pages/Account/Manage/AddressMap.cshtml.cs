using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace DeliveryWebApp.WebUI.Areas.Identity.Pages.Account.Manage
{
    public class AddressMapModel : PageModel
    {

        public string JsonString { get; set; }

        public void OnGet()
        {
        }

        public IActionResult onPost()
        {
            return RedirectToPage("./Address", routeValues: JsonString);
        }
    }
}
