using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Orders.Commands.UpdateOrder;
using DeliveryWebApp.Application.Riders.Commands.UpdateRider;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.RiderPages
{
    [Authorize(Policy = PolicyName.IsRider)]
    public class DirectionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public DirectionsModel(ApplicationDbContext context, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mediator = mediator;
            _userManager = userManager;
        }

        public int Id { get; set; }
        public Order Order { get; set; }
        public Rider Rider { get; set; }
        public Restaurateur Restaurateur { get; set; }

        public async Task<IActionResult> OnGetASync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id, user);

            if (Order == null || Restaurateur == null)
            {
                return NotFound("Unable to load entities");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSuccessAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id, user);

            if (Order == null || Restaurateur == null)
            {
                return NotFound("Unable to load entities");
            }

            await _mediator.Send(new UpdateOrderCommand
            {
                DeliveryDate = DateTime.UtcNow,
                Id = Order.Id,
                OrderStatus = OrderStatus.Delivered
            });

            await _mediator.Send(new UpdateTotalCredit
            {
                Id = Rider.Id
            });


            return Redirect("/RiderPages/DeliveryHistory");
        }

        public async Task<IActionResult> OnPostFailedAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id, user);

            if (Order == null || Restaurateur == null)
            {
                return NotFound("Unable to load entities");
            }

            await _mediator.Send(new UpdateOrderCommand
            {
                Id = Order.Id,
                OrderStatus = OrderStatus.Failed
            });

            return Redirect("/RiderPages/DeliveryHistory");
        }

        private async Task LoadAsync(int id, ApplicationUser user)
        {
            Order = await _context.Orders.FindAsync(id);

            if (Order != null)
            {
                Restaurateur = await _context.Restaurateurs.FindAsync(Order.RestaurateurId);

            }

            try
            {
                var customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);

                Rider = await _context.Riders.FirstAsync(r => r.CustomerId == customer.Id);
            }
            catch (InvalidOperationException)
            {
                Rider = null;
            }
        }
    }
}
