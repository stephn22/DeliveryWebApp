using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur;
using DeliveryWebApp.Application.Restaurateurs.Extensions;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.RestaurateurPages
{
    [Authorize(Policy = PolicyName.IsRestaurateur)]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    public class RestaurantDashboardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RestaurantDashboardModel> _logger;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<RestaurantDashboardModel> _localizer;

        public RestaurantDashboardModel(ApplicationDbContext context, ILogger<RestaurantDashboardModel> logger,
            IMediator mediator, UserManager<ApplicationUser> userManager,
            IStringLocalizer<RestaurantDashboardModel> localizer)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _localizer = localizer;
        }

        public bool HasRestaurant { get; set; }
        public Address RestaurantAddress { get; set; }
        public Restaurateur Restaurateur { get; set; }

        [TempData] public string StatusMessage { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> Categories => new[]
        {
            new SelectListItem { Text = _localizer["Select a category"], Value = "---", Selected = true },
            new SelectListItem { Text = RestaurantCategory.FastFood, Value = RestaurantCategory.FastFood },
            new SelectListItem { Text = RestaurantCategory.Sushi, Value = RestaurantCategory.Sushi },
            new SelectListItem { Text = RestaurantCategory.Indian, Value = RestaurantCategory.Indian },
            new SelectListItem { Text = RestaurantCategory.Italian, Value = RestaurantCategory.Italian },
            new SelectListItem { Text = RestaurantCategory.Chinese, Value = RestaurantCategory.Chinese },
            new SelectListItem { Text = RestaurantCategory.Pizzeria, Value = RestaurantCategory.Pizzeria },
        };

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [Required] [DataType(DataType.Upload)] public IFormFile Logo { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [StringLength(120, MinimumLength = 3)]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Text)] public string Category { get; set; }

            [Required]
            [DataType(DataType.Text)]
            public string PlaceName { get; set; }

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

        // FIXME: not triggered
        private async Task<IActionResult> OnPostAsync()
        {
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

            var address = await _mediator.Send(new CreateAddressCommand
            {
                RestaurateurId = Restaurateur.Id,
                Latitude = Input.Latitude,
                Longitude = Input.Longitude,
                PlaceName = Input.PlaceName
            });

            _logger.LogInformation($"Created address with id: {address.Id}");

            var id = await _mediator.Send(new UpdateRestaurateurCommand
            {
                Id = Restaurateur.Id,
                RestaurantAddress = address,
                Logo = bytes,
                RestaurantCategory = Input.Category,
                RestaurantName = Input.Name
            });

            _logger.LogInformation($"Updated restaurateur with id: {id}");
            StatusMessage = _localizer["Your restaurant has been created successfully"];

            return Page();
        }
    }
}