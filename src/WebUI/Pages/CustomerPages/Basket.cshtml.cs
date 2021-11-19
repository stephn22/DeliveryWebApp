using DeliveryWebApp.Application.BasketItems.Commands.DeleteBasketItem;
using DeliveryWebApp.Application.BasketItems.Commands.UpdateBasketItem;
using DeliveryWebApp.Application.Baskets.Queries;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Models;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using Duende.IdentityServer.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.CustomerPages
{
    [Authorize(Policy = PolicyName.IsCustomer)]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    public class BasketModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BasketModel> _logger;
        private readonly IStringLocalizer<BasketModel> _localizer;
        private readonly IConfiguration _configuration;

        public BasketModel(ApplicationDbContext context, IMediator mediator, UserManager<ApplicationUser> userManager,
            ILogger<BasketModel> logger, IStringLocalizer<BasketModel> localizer, IConfiguration configuration)
        {
            _context = context;
            _mediator = mediator;
            _userManager = userManager;
            _logger = logger;
            _localizer = localizer;
            _configuration = configuration;
        }

        public Basket Basket { get; set; }
        public PaginatedList<BasketItem> BasketItems { get; set; }
        public List<Product> Products { get; set; }
        public Customer Customer { get; set; }
        public Restaurateur Restaurateur { get; set; }

        [TempData] public string StatusMessage { get; set; }

        [BindProperty] [Required] public int NewQuantity { get; set; }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found");
            }

            await LoadAsync(user, pageIndex);

            if (Customer == null)
            {
                return NotFound("Unable to find entities");
            }

            return Page();
        }

        private async Task LoadAsync(ApplicationUser user, int? pageIndex)
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

            Basket = await _mediator.Send(new GetBasketQuery
            {
                Customer = Customer
            });

            if (Basket != null)
            {
                var basketItems = _context.BasketItems.Where(b => b.BasketId == Basket.Id);

                var pageSize = _configuration.GetValue("PageSize", 5);
                BasketItems =
                    await PaginatedList<BasketItem>.CreateAsync(basketItems.AsNoTracking(), pageIndex ?? 1, pageSize);

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

                    if (!Products.IsNullOrEmpty())
                    {
                        Restaurateur = await _context.Restaurateurs.FindAsync(Products[0].RestaurateurId);
                    }
                }
            }
        }

        public async Task<IActionResult> OnPostRemoveFromBasketAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found");
            }

            await LoadAsync(user, 1);

            if (Customer == null || Basket == null || Products.IsNullOrEmpty() || BasketItems.IsNullOrEmpty())
            {
                return NotFound("Unable to find entities");
            }

            try
            {
                await _mediator.Send(new DeleteBasketItemCommand
                {
                    Id = id
                });

                _logger.LogInformation($"Deleted basket item with id: '{id}'");

                StatusMessage = _localizer["Successfully removed item from basket"];
            }
            catch (NotFoundException e)
            {
                _logger.LogError($"Unable to find basket item with id '{id}': {e.Message}");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateBasketItemAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound("User not found");
            }

            await LoadAsync(user, 1);

            if (Customer == null || Basket == null || Products.IsNullOrEmpty() || BasketItems.IsNullOrEmpty())
            {
                return NotFound("Unable to find entities");
            }

            try
            {
                await _mediator.Send(new UpdateBasketItemCommand
                {
                    Id = id,
                    Quantity = NewQuantity
                });

                _logger.LogInformation($"Deleted basket item with id: '{id}'");

                StatusMessage = _localizer["Successfully updated basket"];
            }
            catch (NotFoundException e)
            {
                _logger.LogError($"Unable to find basket item with id '{id}': {e.Message}");
            }

            return RedirectToPage();
        }
    }
}