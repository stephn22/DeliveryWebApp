using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;
using DeliveryWebApp.Application.Baskets.Commands.UpdateBasket;
using DeliveryWebApp.Application.Baskets.Queries;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Customers.Extensions;
using DeliveryWebApp.Application.Restaurateurs.Extensions;
using DeliveryWebApp.Application.Reviews.Commands.CreateReview;
using DeliveryWebApp.Application.Reviews.Queries.GetReviews;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Models;
using Microsoft.Extensions.Configuration;

namespace DeliveryWebApp.WebUI.Pages.CustomerPages
{
    [Authorize(Policy = PolicyName.IsCustomer)]
    public class RestaurantDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RestaurantDetailModel> _logger;
        private readonly IStringLocalizer<RestaurantDetailModel> _localizer;
        private readonly IConfiguration _configuration;

        public RestaurantDetailModel(ApplicationDbContext context, IMediator mediator,
            UserManager<ApplicationUser> userManager, ILogger<RestaurantDetailModel> logger, IStringLocalizer<RestaurantDetailModel> localizer,
            IConfiguration configuration)
        {
            _context = context;
            _mediator = mediator;
            _userManager = userManager;
            _logger = logger;
            _localizer = localizer;
            _configuration = configuration;
        }

        public Customer Customer { get; set; }
        public Basket Basket { get; set; }
        public Restaurateur Restaurateur { get; set; }
        public List<Product> Products { get; set; }

        /// <summary>
        /// Average rating for the current food vendor
        /// </summary>
        public double AverageRating { get; set; }

        // how many reviews the customer can post
        public int AvailableReviews { get; set; }

        // all reviews of this restaurateur
        public PaginatedList<Review> Reviews { get; set; }

        // for products
        public string CurrentFilter { get; set; }

        [BindProperty] public InputModel Input { get; set; }


        public class InputModel
        {
            [Required]
            [DisplayName("Quantity")]
            public int Quantity { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [DisplayName("Title")]
            public string Title { get; set; }

            [DataType(DataType.Text)]
            [StringLength(250)]
            [DisplayName("Text")]
            public string Text { get; set; }

            [Required]
            [Range(1, 5)]
            public int Rating { get; set; }
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id, string searchString, string currentFilter, int? pageIndex)
        {
            if (id == null)
            {
                return NotFound("Unable to find food vendor with that id");
            }

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            var user = await _userManager.GetUserAsync(User);
            await LoadAsync(user, id);

            if (Restaurateur == null)
            {
                return NotFound("Unable to find food vendor with that id");
            }

            var products = _context.Products.Where(p => p.RestaurateurId == Restaurateur.Id);

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.ToLower().Contains(searchString.ToLower()));
            }

            Products = await products.AsNoTracking().ToListAsync();

            var pageSize = _configuration.GetValue("PageSize", 5);
            var reviews = _context.Reviews.Where(r => r.RestaurateurId == Restaurateur.Id);
            Reviews = await PaginatedList<Review>.CreateAsync(reviews.AsNoTracking(), pageIndex ?? 1, pageSize);

            return Page();
        }

        private async Task LoadAsync(ApplicationUser user, int? id)
        {
            Restaurateur = await _context.Restaurateurs.FindAsync(id);

            if (Restaurateur != null)
            {
                AverageRating = await Restaurateur.GetRestaurateurAverageRating(_mediator);

                try
                {
                    Customer = await _context.Customers.FirstAsync(c => c.ApplicationUserFk == user.Id);

                    

                    // check if customer can review the restaurateur
                    AvailableReviews = await Customer.GetAvailableReviews(_mediator);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogInformation($"Unable to find customer: {e.Message}");
                    Customer = null;
                    return;
                }

                Basket = await _mediator.Send(new GetBasketQuery
                {
                    Customer = Customer
                }) // If basket does not exist (null), create a new one
                         ?? await _mediator.Send(new CreateBasketCommand
                         {
                             Customer = Customer
                         });

                _logger.LogInformation($"Created new basket with id: {Basket.Id}");
            }
        }

        public async Task<IActionResult> OnPostAddToBasketAsync(int? id, int productId)
        {
            if (id == null)
            {
                return NotFound("Unable to find food vendor with that id");
            }

            var user = await _userManager.GetUserAsync(User);

            await LoadAsync(user, id);

            if (Restaurateur == null || Customer == null)
            {
                return NotFound("Unable to find entities");
            }

            var product = Products.First(p => p.Id == productId);

            await _mediator.Send(new UpdateBasketCommand
            {
                Basket = Basket,
                Product = product,
                Quantity = Input.Quantity,
                RestaurateurId = Restaurateur.Id
            });

            _logger.LogInformation($"Added product with id {product.Id} to the basket of the user {user.Id}");

            StatusMessage = _localizer["Added product to cart successfully"];

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostAddReviewAsync(int? id)
        {
            if (id == null)
            {
                return NotFound("Unable to find food vendor with that id");
            }

            var user = await _userManager.GetUserAsync(User);

            await LoadAsync(user, id);

            if (Restaurateur == null || Customer == null)
            {
                return NotFound("Unable to find entities");
            }

            var review = await _mediator.Send(new CreateReviewCommand
            {
                Customer = Customer,
                Rating = Input.Rating,
                Restaurateur = Restaurateur,
                Text = Input.Text,
                Title = Input.Title
            });

            _logger.LogInformation($"Added review with id: {review.Id}");

            return RedirectToPage();
        }
    }
}
