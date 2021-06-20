using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeliveryWebApp.WebUI.Pages.Admin
{
    [Authorize(Roles = RoleName.Admin)]
    public class UserListModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
