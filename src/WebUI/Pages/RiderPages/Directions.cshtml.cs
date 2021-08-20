using System;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Orders.Commands.UpdateOrder;
using DeliveryWebApp.Domain.Constants;

namespace DeliveryWebApp.WebUI.Pages.RiderPages
{
    [Authorize(Policy = PolicyName.IsRider)]
    public class DirectionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public DirectionsModel(ApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public Order Order { get; set; }
        public Restaurateur Restaurateur { get; set; }

        public async Task<IActionResult> OnGetASync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

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

            await LoadAsync((int)id);

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

            return RedirectToPage(); // TODO: redirect to rider dashboard
        }

        public async Task<IActionResult> OnPostFailedAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

            if (Order == null || Restaurateur == null)
            {
                return NotFound("Unable to load entities");
            }

            await _mediator.Send(new UpdateOrderCommand
            {
                Id = Order.Id,
                OrderStatus = OrderStatus.Failed
            });

            return RedirectToPage(); // TODO: redirect to rider dashboard
        }

        private async Task LoadAsync(int id)
        {
            Order = await _context.Orders.FindAsync(id);

            if (Order != null)
            {
                Restaurateur = await _context.Restaurateurs.FindAsync(Order.RestaurateurId);

            }
        }
    }
}
