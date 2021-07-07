using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Products.Queries.GetProducts;
using DeliveryWebApp.Application.Restaurants.Commands.CreateRestaurant;
using DeliveryWebApp.Application.Restaurants.Commands.UpdateRestaurant;
using DeliveryWebApp.Application.Restaurants.Extensions;
using DeliveryWebApp.Application.Restaurateurs.Extensions;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    // TODO: localize
    [Authorize(Policy = PolicyName.IsRestaurateur)]
    public class RestaurantDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RestaurantDashboardModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public RestaurantDashboardModel(ApplicationDbContext context, ILogger<RestaurantDashboardModel> logger,
            IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }

        public Restaurant Restaurant { get; set; }
        public Address RestaurantAddress { get; set; }
        public Restaurateur Restaurateur { get; set; }
        public IList<Product> Products { get; set; }
        public IList<Order> Orders { get; set; }

        public string NameSort { get; set; }
        public string CategorySort { get; set; }
        public string PriceSort { get; set; }
        public string DiscountSort { get; set; }
        public string QuantitySort { get; set; }

        [TempData] public string StatusMessage { get; set; }

        public SelectList Countries => new(Utilities.CountryList(), "Key", "Value");

        [BindProperty]
        public IEnumerable<SelectListItem> Categories => new[]
        {
            new SelectListItem {Text = "Select a category", Value = "", Selected = true},
            new SelectListItem {Text = RestaurantCategory.FastFood, Value = RestaurantCategory.FastFood},
            new SelectListItem {Text = RestaurantCategory.Sushi, Value = RestaurantCategory.Sushi},
            new SelectListItem {Text = RestaurantCategory.Indian, Value = RestaurantCategory.Indian},
            new SelectListItem {Text = RestaurantCategory.Italian, Value = RestaurantCategory.Italian},
            new SelectListItem {Text = RestaurantCategory.Chinese, Value = RestaurantCategory.Chinese},
            new SelectListItem {Text = RestaurantCategory.Pizzeria, Value = RestaurantCategory.Pizzeria},
        };

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required] [DataType(DataType.Upload)] public IFormFile Logo { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [StringLength(120, MinimumLength = 3)]
            public string Name { get; set; }

            [DataType(DataType.Text)] public string Category { get; set; }

            /******************* ADDRESS *****************/

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Address Line 1")]
            public string AddressLine1 { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Address Line 2")]
            public string AddressLine2 { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Number")]
            public string Number { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "City")]
            public string City { get; set; }

            [Required]
            [DataType(DataType.PostalCode)]
            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

            [Required]
            [DataType((DataType.Text))]
            [Display(Name = "State/Province")]
            public string StateProvince { get; set; }

            [Required] [DataType(DataType.Text)] public string Country { get; set; }

            public double Longitude { get; set; }
            public double Latitude { get; set; }

            /*********************************************/
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            Restaurateur = await _context.GetRestaurateurByApplicationUserFkAsync(user.Id);

            Restaurant = await _context.GetRestaurantByRestaurateurId(Restaurateur.Id);

            RestaurantAddress = await _context.GetRestaurantAddress(Restaurant);

            if (Restaurant != null)
            {
                //Products = await _mediator.Send(new GetProductsQuery
                //{
                //    RestaurantId = Restaurant.Id
                //});

                //Orders = Restaurant.Orders.ToList();
            }
        }

        public async Task<IActionResult> OnGetAsync(string sortOrder)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            CategorySort = string.IsNullOrEmpty(sortOrder) ? "category_desc" : "";
            PriceSort = string.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            DiscountSort = string.IsNullOrEmpty(sortOrder) ? "discount_desc" : "";
            QuantitySort = string.IsNullOrEmpty(sortOrder) ? "quantity_desc" : "";

            var products = from p in _context.Products
                where p.RestaurantId == Restaurant.Id
                select p;

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(p => p.Name);
                    break;

                case "category_desc":
                    products = products.OrderByDescending(p => p.Category);
                    break;

                case "price_desc":
                    products = products.OrderByDescending(p => p.Price);
                    break;

                case "discount_desc":
                    products = products.OrderByDescending(p => p.Discount);
                    break;

                case "quantity_desc":
                    products = products.OrderByDescending(p => p.Quantity);
                    break;
            }

            Products = await products.AsNoTracking().ToListAsync();

            return Page();
        }

        /// <summary>
        /// Restaurant has changed name
        /// </summary>
        /// <param name="id">restaurant id</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostNewNameAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await LoadAsync(user);

            try
            {
                await _mediator.Send(new UpdateRestaurantCommand
                {
                    Id = id,
                    Name = Input.Name
                });
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e.Message);
            }

            StatusMessage =
                "Your restaurant name has been updated. It may take a few moments to update across the site.";

            return Page();
        }

        public async Task<IActionResult> OnPostNewCategoryAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            await LoadAsync(user);

            // if no category is selected
            if (Categories.First(c => c.Value == "").Selected)
            {
                return Page();
            }

            try
            {
                await _mediator.Send(new UpdateRestaurantCommand
                {
                    Id = id,
                    Category = Input.Category
                });
            }
            catch (NotFoundException e)
            {
                _logger.LogError(e.Message);
            }

            StatusMessage =
                "Your restaurant category has been updated. It may take a few moments to update across the site.";

            return Page();
        }

        public async Task<IActionResult> OnPostUploadNewImageAsync()
        {
            if (Input?.Logo == null) return Page();

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            byte[] bytes;

            await using var fileStream = Input.Logo.OpenReadStream();
            await using (var memoryStream = new MemoryStream())
            {
                await fileStream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }

            await _mediator.Send(new UpdateRestaurantCommand
            {
                Id = Restaurant.Id,
                Logo = bytes
            });

            StatusMessage =
                "Your restaurant picture has been updated. It may take a few moments to update across the site.";
            return Page();
        }

        public async Task<IActionResult> OnPostNewAddressAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            await _mediator.Send(new UpdateRestaurantCommand
            {
                Id = Restaurant.Id,
                Address = new Address
                {
                    AddressLine1 = Input.AddressLine1,
                    AddressLine2 = Input.AddressLine2,
                    City = Input.City,
                    Country = Input.Country,
                    StateProvince = Input.StateProvince,
                    Number = Input.Number,
                    PostalCode = Input.PostalCode,
                    Latitude = Input.Latitude,
                    Longitude = Input.Longitude
                }
            });

            return Page();
        }

        public async Task<IActionResult> OnPostNewRestaurantAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            await LoadAsync(user);

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.Logo.Length <= 0)
            {
                return Page();
            }

            byte[] bytes;

            await using var fileStream = Input.Logo.OpenReadStream();
            await using (var memoryStream = new MemoryStream())
            {
                await fileStream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }

            RestaurantAddress = new Address
            {
                AddressLine1 = Input.AddressLine1,
                AddressLine2 = Input.AddressLine2,
                City = Input.City,
                Country = Input.Country,
                StateProvince = Input.StateProvince,
                Number = Input.Number,
                PostalCode = Input.PostalCode,
                Latitude = Input.Latitude,
                Longitude = Input.Longitude
            };

            // insert new restaurant in context
            var restaurant = await _mediator.Send(new CreateRestaurantCommand
            {
                Address = RestaurantAddress,
                Category = Input.Category,
                Logo = bytes,
                Name = Input.Name,
                Restaurateur = Restaurateur
            });

            _logger.LogInformation($"Created new restaurant with id: {restaurant.Id}");

            StatusMessage =
                "Your restaurant has been created successfully";

            return RedirectToPage();
        }
    }
}