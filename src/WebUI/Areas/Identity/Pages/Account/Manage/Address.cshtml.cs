using DeliveryWebApp.Application.Addresses.Queries.GetAddresses;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using DeliveryWebApp.Application.Customers.Commands.UpdateCustomer;

namespace DeliveryWebApp.WebUI.Areas.Identity.Pages.Account.Manage
{
    // TODO: delete address
    [Authorize(Policy = PolicyName.IsCustomer)]
    public partial class AddressModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AddressModel> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;

        public AddressModel(UserManager<ApplicationUser> userManager, ILogger<AddressModel> logger,
            ApplicationDbContext context, IMediator mediator)
        {
            _userManager = userManager;
            _logger = logger;
            _context = context;
            _mediator = mediator;
        }

        [TempData] public string StatusMessage { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public List<Address> Addresses { get; set; }

        public Customer Customer { get; set; }

        public SelectList Countries => new(Utilities.CountryList(), "Key", "Value");

        public class InputModel
        {
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
            [DataType(DataType.Text)]
            public string Country { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Customer = await _context.Customers.Where(c => c.ApplicationUserFk == user.Id).FirstAsync();

            await LoadAddressesAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAddressAsync(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadAddressesAsync(user);
                return Page();
            }

            await _mediator.Send(new UpdateAddressCommand
            {
                Id = id,
                AddressLine1 = Input.AddressLine1,
                AddressLine2 = Input.AddressLine2,
                City = Input.City,
                Country = Input.Country,
                Number = Input.Number,
                PostalCode = Input.PostalCode
            });

            _logger.LogInformation($"Updated address with id '{id}'.");
            StatusMessage = "Your address has been updated";

            return RedirectToPage();
        }

        /// <summary>
        /// Called when customer add new address for the first time
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await LoadAddressesAsync(user);
                return Page();
            }

            var customer = await (from c in _context.Customers
                where c.ApplicationUserFk == user.Id
                select c).FirstAsync();

            var address = new Address
            {
                AddressLine1 = Input.AddressLine1,
                AddressLine2 = Input.AddressLine2,
                City = Input.City,
                Country = Input.Country,
                Number = Input.Number,
                PostalCode = Input.PostalCode
            };

            await _mediator.Send(new UpdateCustomerCommand
            {
                Id = customer.Id,
                Address = address
            });

            StatusMessage = "Your address has been updated";
            return RedirectToPage();
        }

        private async Task LoadAddressesAsync(ApplicationUser user)
        {
            var customer = await (from c in _context.Customers
                                   where c.ApplicationUserFk == user.Id
                                   select c).FirstAsync();

            Addresses = await _mediator.Send(new GetAddressesQuery
            {
                CustomerId = customer.Id
            });
        }
    }
}