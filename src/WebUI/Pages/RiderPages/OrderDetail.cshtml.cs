using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.OrderItems.Extensions;
using DeliveryWebApp.Application.OrderItems.Queries;
using DeliveryWebApp.Application.Orders.Commands.UpdateOrder;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.RiderPages
{
    [Authorize(Policy = PolicyName.IsRider)]
    public class OrderDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<OrderDetailModel> _logger;

        public OrderDetailModel(ApplicationDbContext context, IMediator mediator,
            UserManager<ApplicationUser> userManager, ILogger<OrderDetailModel> logger)
        {
            _context = context;
            _mediator = mediator;
            _userManager = userManager;
            _logger = logger;
        }

        public Order Order { get; set; }
        public Restaurateur Restaurateur { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public List<Product> Products { get; set; }
        public Address DeliveryAddress { get; set; }
        public Rider Rider { get; set; }

        /// <summary>
        /// This order has been taken by another rider
        /// </summary>
        public bool HasBeenTaken { get; set; }

        private async Task LoadAsync(int id, ApplicationUser user)
        {
            Order = await _context.Orders.FindAsync(id);

            if (Order != null)
            {
                HasBeenTaken = (Order.Status == OrderStatus.Taken);

                Restaurateur = await _context.Restaurateurs.FindAsync(Order.RestaurateurId);
                DeliveryAddress = await _context.Addresses.FindAsync(Order.DeliveryAddressId);

                OrderItems = await _mediator.Send(new GetOrderItemsQuery
                {
                    OrderId = Order.Id
                });

                if (OrderItems != null)
                {
                    Products = new List<Product>();

                    foreach (var item in OrderItems)
                    {
                        var p = await item.GetProduct(_context);

                        if (p != null)
                        {
                            Products.Add(p);
                        }
                    }
                }
            }

            try
            {
                var customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);

                Rider = await _context.Riders.FirstAsync(r => r.CustomerId == customer.Id);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError(e.Message);
                Rider = null;
            }
        }

        public async Task<IActionResult> OnGet(int? id)
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

            if (Order == null || Restaurateur == null || OrderItems.IsNullOrEmpty() || DeliveryAddress == null ||
                Rider == null)
            {
                return NotFound("Unable to load entities");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
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

            if (Order == null || Restaurateur == null || OrderItems.IsNullOrEmpty() || DeliveryAddress == null)
            {
                return NotFound("Unable to load entities");
            }

            await _mediator.Send(new UpdateOrderCommand
            {
                Id = Order.Id,
                OrderStatus = OrderStatus.Taken,
                Rider = Rider
            });

            _logger.LogInformation($"Updated order with id: {Order.Id}");

            return Redirect($"/RiderPages/Directions/{id}");
        }
    }
}