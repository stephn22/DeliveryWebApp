using DeliveryWebApp.Application.Addresses.Queries.GetAddresses;
using DeliveryWebApp.Application.BasketItems.Queries;
using DeliveryWebApp.Application.Baskets.Queries;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Orders.Commands.CreateOrder;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using Duende.IdentityServer.Extensions;
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
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
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

        public List<Product> Products { get; set; }
        public Restaurateur Restaurateur { get; set; }

        [BindProperty]
        public List<SelectListItem> CustomerAddresses { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [DisplayName("Address")]
            public int AddressId { get; set; }

            [Required]
            [DataType(DataType.CreditCard)]
            [DisplayName("Credit Card")]
            public string CreditCard { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [DisplayName("CVV")]
            public string Cvv { get; set; }

            [Required]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:MM/yyyy}", ApplyFormatInEditMode = true)]
            public DateTime ExpirationDate { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string Name { get; set; }
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

            if (!addresses.IsNullOrEmpty())
            {
                CustomerAddresses = new List<SelectListItem>();

                foreach (var address in addresses)
                {
                    CustomerAddresses.Add(new SelectListItem { Text = address.PlaceName, Value = address.Id.ToString() });
                }

                CustomerAddresses[0].Selected = true;
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

            var order = await _mediator.Send(new CreateOrderCommand
            {
                Customer = Customer,
                BasketItems = BasketItems,
                Restaurateur = Restaurateur,
                AddressId = Input.AddressId
            });

            _logger.LogInformation($"Created new order with id: {order.Id}");

            return Redirect("/CustomerPages/CustomerOrders");
        }
    }
}
