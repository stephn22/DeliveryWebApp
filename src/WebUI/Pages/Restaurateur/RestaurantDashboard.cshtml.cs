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
using System.Net.Mime;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Restaurants.Commands.CreateRestaurant;
using DeliveryWebApp.Application.Restaurants.Commands.UpdateRestaurant;
using DeliveryWebApp.Application.Restaurants.Extensions;
using DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        public Address RestaurantAddress { get; set; }
        public Domain.Entities.Restaurateur Restaurateur { get; set; }

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

        private async Task LoadAsync(ApplicationUser user)
        {
            Restaurateur = await _context.GetRestaurateurByApplicationUserFkAsync(user.Id);

            Restaurant = await _context.GetRestaurantByRestaurateurId(Restaurateur.Id);

            RestaurantAddress = await _context.GetRestaurantAddress(Restaurant);

            if (Restaurant != null)
            {
                foreach (var category in Categories)
                {
                    category.Selected = category.Value == Restaurant.Category;
                }
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            await LoadAsync(user);

            return Page();
        }

        /// <summary>
        /// Restaurant has changed name
        /// </summary>
        /// <param name="id">restaurant id</param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostNewNameAsync(int id)
        {
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
                return NotFound();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostNewCategoryAsync(int id)
        {
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
                return NotFound();
            }

            return RedirectToPage();
        }

        public async Task UploadNewImageAsync()
        {
            if (Input != null)
            {
                byte[] bytes;

                await using var fileStream = Input.Logo.OpenReadStream();
                await using (var memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                await _mediator.Send(new UpdateRestaurantCommand
                {
                    Logo = bytes
                });
            }
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

            if (Input.Logo.Length <= 0) return Page();

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
                Number = Input.Number,
                PostalCode = Input.PostalCode
            };

            // insert new restaurant in context
            var restaurantId = await _mediator.Send(new CreateRestaurantCommand
            {
                Address = RestaurantAddress,
                Category = Input.Category,
                Logo = bytes,
                Name = Input.Name,
                Restaurateur = Restaurateur
            });

            _logger.LogInformation($"Created new restaurant with id: {restaurantId}");

            return RedirectToPage();
        }
    }
}