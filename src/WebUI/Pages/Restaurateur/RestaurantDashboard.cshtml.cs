using System;
using DeliveryWebApp.Application.Restaurateurs.Extensions;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Restaurants.Commands.CreateRestaurant;
using DeliveryWebApp.Application.Restaurants.Extensions;
using DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.WebUI.Pages.Restaurateur
{
    [Authorize(Policy = PolicyName.IsRestaurateur)]
    public class RestaurantDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RestaurantDashboardModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;

        public RestaurantDashboardModel(ApplicationDbContext context, ILogger<RestaurantDashboardModel> logger,
            IMediator mediator, UserManager<ApplicationUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _environment = environment;
        }

        public Restaurant Restaurant { get; set; }
        public Domain.Entities.Restaurateur Restaurateur { get; set; }

        public SelectList Countries => new(Utilities.CountryList(), "Key", "Value");

        [BindProperty]
        public IEnumerable<SelectListItem> Categories => new[]
        {
            new SelectListItem {Text = "Select a category", Value = ""},
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

            [Required] [DataType(DataType.Text)] public string Name { get; set; }

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

            [Required] [DataType(DataType.Text)] public string Country { get; set; }

            /*********************************************/
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            Restaurateur = await _context.GetRestaurateurByApplicationUserFkAsync(user.Id);

            try
            {
                Restaurant = await _context.GetRestaurantByRestaurateurId(Restaurateur.Id);
            }
            catch (InvalidOperationException)
            {
                Restaurant = null;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            Restaurateur = await _context.GetRestaurateurByApplicationUserFkAsync(user.Id);

            if (!ModelState.IsValid)
            {
                // TODO load
                return Page();
            }
            // FIXME: Directory not created
            var directory = Path.Combine($"/Resources/Uploads/Logos/{user.Id}/");
            Directory.CreateDirectory(directory);

            var file = Path.Combine(_environment.ContentRootPath, directory + Input.Logo.FileName);

            using (var fileStream = new FileStream(file, FileMode.Create)) // FIXME: DirectoryNotFoundException
            {
                await Input.Logo.CopyToAsync(fileStream);
            }

            var restaurantAddress = new Address
            {
                AddressLine1 = Input.AddressLine1,
                AddressLine2 = Input.AddressLine2,
                City = Input.City,
                Country = Input.Country,
                Number = Input.Number,
                PostalCode = Input.PostalCode
            };

            // insert new address in context
            var addressId = await _mediator.Send(new CreateAddressCommand
            {
                AddressLine1 = restaurantAddress.AddressLine1,
                AddressLine2 = restaurantAddress.AddressLine2,
                City = restaurantAddress.City,
                Country = restaurantAddress.Country,
                Number = restaurantAddress.Number,
                PostalCode = restaurantAddress.PostalCode
            });

            _logger.LogInformation($"Created new address with id: {addressId}");

            // insert new restaurant in context
            var restaurantId = await _mediator.Send(new CreateRestaurantCommand
            {
                Address = restaurantAddress,
                Category = Input.Category,
                LogoUrl = file,
                Name = Input.Name,
                Restaurateur = Restaurateur
            });

            var restaurant = await _context.Restaurants.FindAsync(restaurantId);

            // update restaurateur with his new restaurant

            await _mediator.Send(new UpdateRestaurateurCommand
            {
                Id = Restaurateur.Id,
                Restaurant = restaurant
            });

            _logger.LogInformation($"Created new restaurant with id: {restaurantId}");

            return RedirectToPage();
        }
    }
}