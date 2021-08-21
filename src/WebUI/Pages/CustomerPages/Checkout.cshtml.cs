using DeliveryWebApp.Application.Addresses.Queries.GetAddresses;
using DeliveryWebApp.Application.BasketItems.Queries;
using DeliveryWebApp.Application.Baskets.Queries;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Orders.Commands.CreateOrder;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using IdentityServer4.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.CustomerPages
{
    [Authorize(Policy = PolicyName.IsCustomer)]
    public class CheckoutModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CheckoutModel> _logger;
        private readonly IMediator _mediator;

        public CheckoutModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, ILogger<CheckoutModel> logger, IMediator mediator)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
        }

        public Basket Basket { get; set; }
        public List<BasketItem> BasketItems { get; set; }
        public Customer Customer { get; set; }
        public List<SelectListItem> CustomerAddresses { get; set; }
        public List<Product> Products { get; set; }
        public Restaurateur Restaurateur { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [DisplayName("Address")]
            public Address Address { get; set; }

            [Required]
            [DataType(DataType.CreditCard)]
            [DisplayName("Credit Card")]
            public string CreditCard { get; set; }
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(user);

            if (Customer == null && CustomerAddresses == null && Basket == null)
            {
                return NotFound("Unable to find entities");
            }

            return Page();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            try
            {
                Customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Customer not found: {e.Message}");
                Customer = null;
                return;
            }

            var addresses = await _mediator.Send(new GetAddressesQuery
            {
                CustomerId = Customer.Id
            });

            CustomerAddresses = new List<SelectListItem>
            {
                new() { Text = addresses[0].PlaceName, Value = addresses[0].PlaceName },
            };

            if (addresses.Count == 2)
            {
                CustomerAddresses.Add(new SelectListItem
                {
                    Text = addresses[1].PlaceName,
                    Value = addresses[1].PlaceName
                });
            }

            Basket = await _mediator.Send(new GetBasketQuery
            {
                Customer = Customer
            });

            if (Basket != null)
            {
                BasketItems = await _mediator.Send(new GetBasketItemsQuery
                {
                    Basket = Basket
                });

                if (BasketItems != null)
                {
                    Products = new List<Product>();

                    foreach (var item in BasketItems)
                    {
                        var p = await _context.Products.FindAsync(item.ProductId);

                        if (p is { Quantity: > 0 })
                        {
                            Products.Add(p);
                        }
                        else // remove from basket items products with quantity == 0
                        {
                            BasketItems.RemoveAll(b => b.ProductId == p.Id);
                        }
                    }
                }

                if (!Products.IsNullOrEmpty())
                {
                    Restaurateur = await _context.Restaurateurs.FindAsync(Products[0].RestaurateurId);
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(user);

            if (Customer == null && CustomerAddresses == null && Basket == null)
            {
                return NotFound("Unable to find entities");
            }

            try
            {
                var order = await _mediator.Send(new CreateOrderCommand
                {
                    Customer = Customer,
                    BasketItems = BasketItems,
                    Restaurateur = Restaurateur,
                    Address = Input.Address
                });

                _logger.LogInformation($"Created new order with id: {order.Id}");
            }
            catch (NotFoundException e)
            {
                _logger.LogError($"Basket not found: {e.Message}");
            }

            return Redirect("/CustomerPages/CustomerOrders");
        }
    }
}
