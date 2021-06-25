using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Clients.Queries.GetClients;
using DeliveryWebApp.Infrastructure.Services.Utilities;

namespace DeliveryWebApp.WebUI.Pages.Admin
{
    [Authorize(Roles = RoleName.Admin)]
    public class RequestDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RequestDetailModel> _logger;

        public RequestDetailModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<RequestDetailModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        public Request UserRequest { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ClaimValue { get; set; }
        public bool IsRider { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Currency)]
            [Display(Name = "Delivery Credit")]
            public double DeliveryCredit { get; set; } // TODO: Culture
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // get client by request id
            var client = await _context.GetClientByRequestIdAsync(id);

            // get request instance
            UserRequest = await (from r in _context.Requests
                                 where r.Id == id
                                 select r).FirstOrDefaultAsync();

            var appUserFk = client.ApplicationUserFk;

            ApplicationUser = await _userManager.FindByIdAsync(appUserFk);
            ClaimValue = await _userManager.GetRoleAsync(ApplicationUser);

            IsRider = UserRequest.Role.Equals(RoleName.Rider);

            if (UserRequest == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAcceptBtnAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            UserRequest = await (from r in _context.Requests
                where r.Id == id
                select r).FirstOrDefaultAsync();

            IsRider = UserRequest.Role.Equals(RoleName.Rider);
            await _context.GetClientByRequestIdAsync(id);

            // update tables
            if (IsRider)
            {
                _context.Riders.Add(new Rider()
                {
                    Client = UserRequest.Client,
                    DeliveryCredit = Input.DeliveryCredit
                });
            }
            else
            {
                _context.Restaurateurs.Add(new Domain.Entities.Restaurateur
                {
                    Client = UserRequest.Client,
                });
            }

            UserRequest.Status = RequestStatus.Accepted;

            // update request table
            _context.Requests.Update(UserRequest);

            // TODO push notification to client

            await _context.SaveChangesAsync();
            return RedirectToPage("/Admin/Requests");
        }

        public async Task<IActionResult> OnPostRejectBtnAsync(int? id)
        {
            UserRequest.Status = RequestStatus.Rejected;
            Input.DeliveryCredit = 0.00;

            // update request table
            _context.Requests.Update(UserRequest);

            await _context.SaveChangesAsync();

            // TODO push notification to client
            return RedirectToPage("/Admin/Requests");
        }
    }
}
