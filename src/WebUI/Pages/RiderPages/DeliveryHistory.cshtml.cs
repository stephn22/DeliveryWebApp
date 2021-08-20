using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Orders.Extensions;

namespace DeliveryWebApp.WebUI.Pages.RiderPages
{
    [Authorize(Policy = PolicyName.IsRider)]
    public class DeliveryHistoryModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeliveryHistoryModel(ApplicationDbContext context, IMediator mediator,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mediator = mediator;
            _userManager = userManager;
        }

        public Rider Rider { get; set; }
        public List<Order> Orders { get; set; }
        public List<Restaurateur> Restaurateurs { get; set; }

        private async Task LoadAsync(ApplicationUser user)
        {
            try
            {
                var customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);

                Rider = await _context.Riders.FirstAsync(r => r.CustomerId == customer.Id);

                Orders = await _context.Orders.Where(o => o.RiderId == Rider.Id).ToListAsync();

                if (!Orders.IsNullOrEmpty())
                {
                    Restaurateurs = new List<Restaurateur>();
                    foreach (var order in Orders)
                    {
                        var r = await order.GetRestaurateurAsync(_context);

                        Restaurateurs.Add(r);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                Rider = null;
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(user);

            if (Rider == null)
            {
                return NotFound("Unable to load entities");
            }

            return Page();
        }
    }
}