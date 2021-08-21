using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur;
using DeliveryWebApp.Application.Restaurateurs.Extensions;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Policy = PolicyName.IsRestaurateur)]
    public class RestaurantDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RestaurantDashboardModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<RestaurantDashboardModel> _stringLocalizer;

        public RestaurantDashboardModel(ApplicationDbContext context, ILogger<RestaurantDashboardModel> logger, IMediator mediator, UserManager<ApplicationUser> userManager, IStringLocalizer<RestaurantDashboardModel> stringLocalizer)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _stringLocalizer = stringLocalizer;
        }

        public bool HasRestaurant { get; set; }
        public Address RestaurantAddress { get; set; }
        public Restaurateur Restaurateur { get; set; }

        [TempData] public string StatusMessage { get; set; }

        public SelectList Countries => new(Utilities.CountryList(), "Key", "Value");

        [BindProperty]
        public IEnumerable<SelectListItem> Categories => new[]
        {
            new SelectListItem {Text = _stringLocalizer["Select a category"], Value = "", Selected = true},
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
            [Display(Name = "N°")]
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

            public decimal Longitude { get; set; }
            public decimal Latitude { get; set; }

            /*********************************************/
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);

            return Page();
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            Restaurateur = await _context.GetRestaurateurByApplicationUserFkAsync(user.Id);

            if (Restaurateur != null)
            {
                try
                {
                    RestaurantAddress = await _context.Addresses.FirstAsync(a => a.RestaurateurId == Restaurateur.Id);
                }
                catch (InvalidOperationException e)
                {
                    _logger.LogWarning($"{e.Message}");
                    RestaurantAddress = null;
                }

                HasRestaurant = RestaurantAddress != null && Restaurateur.Logo != null &&
                                Restaurateur.RestaurantName != null && Restaurateur.RestaurantCategory != null;
            }
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

            await LoadAsync(user);

            try
            {
                await _mediator.Send(new UpdateRestaurateurCommand
                {
                    Id = id,
                    RestaurantName = Input.Name
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

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(user);

            try
            {
                await _mediator.Send(new UpdateRestaurateurCommand
                {
                    Id = id,
                    RestaurantCategory = Input.Category
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
            if (Input?.Logo == null)
            {
                return Page();
            }

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

            await _mediator.Send(new UpdateRestaurateurCommand
            {
                Id = Restaurateur.Id,
                Logo = bytes
            });

            StatusMessage =
                "Your restaurant picture has been updated. It may take a few moments to update across the site.";
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(user);

            byte[] bytes;

            await using var fileStream = Input.Logo.OpenReadStream();
            await using (var memoryStream = new MemoryStream())
            {
                await fileStream.CopyToAsync(memoryStream);
                bytes = memoryStream.ToArray();
            }

            try
            {
                await LoadAsync(user);
                var id = await _mediator.Send(new UpdateRestaurateurCommand
                {
                    Id = Restaurateur.Id,
                    RestaurantAddress = RestaurantAddress,
                    RestaurantCategory = Input.Category,
                    Logo = bytes,
                    RestaurantName = Input.Name
                });

                _logger.LogInformation($"Created new restaurant with id: {id}");

                StatusMessage =
                    "Your restaurant has been created successfully";
            }
            catch (NotFoundException e)
            {
                _logger.LogError($"Could not find restaurateur that id: ${e.Message}");
                return NotFound("Could not find restaurateur that id.");
            }

            return RedirectToPage();
        }
    }
}