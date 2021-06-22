using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace DeliveryWebApp.WebUI.Pages.Client
{
    [Authorize(Policy = PolicyName.IsClient)]
    public class RequestModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RequestModel> _logger;
        private readonly ApplicationDbContext _context;

        public RequestModel(UserManager<ApplicationUser> userManager, ILogger<RequestModel> logger, ApplicationDbContext context)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public SelectListItem Roles { get; set; }

        public class InputModel
        {
            [DataType(DataType.Text)]
            [Required]
            public string Role { get; set; }
        }

        public void OnGet()
        {
        }
    }
}
