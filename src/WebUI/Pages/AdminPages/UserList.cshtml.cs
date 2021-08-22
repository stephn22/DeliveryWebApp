using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Customers.Commands.DeleteCustomer;
using DeliveryWebApp.Application.Customers.Queries.GetCustomers;
using DeliveryWebApp.Application.Restaurateurs.Commands.DeleteRestaurateur;
using DeliveryWebApp.Application.Restaurateurs.Queries.GetRestaurateurs;
using DeliveryWebApp.Application.Riders.Commands.DeleteRider;
using DeliveryWebApp.Application.Riders.Queries.GetRiders;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.AdminPages
{
    [Authorize(Roles = RoleName.Admin)]
    public class UserListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserListModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<UserListModel> _localizer;

        public UserListModel(ApplicationDbContext context, ILogger<UserListModel> logger, IMediator mediator,
            UserManager<ApplicationUser> userManager, IStringLocalizer<UserListModel> localizer)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
            _localizer = localizer;
        }

        public IList<Customer> Customers { get; set; }
        public IList<Rider> Riders { get; set; }
        public IList<Restaurateur> Restaurateurs { get; set; }

        [TempData] public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadAsync();

            return Page();
        }

        private async Task LoadAsync()
        {
            Customers = await _mediator.Send(new GetCustomersQuery());
            Riders = await _mediator.Send(new GetRidersQuery());
            Restaurateurs = await _mediator.Send(new GetRestaurateursQuery());
        }

        public async Task<IActionResult> OnPostBlockCustomerAsync(int id)
        {
            try
            {
                var customer = Customers.First(c => c.Id == id);

                var user = await _userManager.FindByIdAsync(customer.ApplicationUserFk);

                // block the user
                await _userManager.BlockUser(user);

                _logger.LogInformation($"Blocked user with id: {user.Id}");
                StatusMessage = _localizer["User blocked successfully"];
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Could not find customer with id {id}: {e.Message}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUnblockCustomerAsync(int id)
        {
            try
            {
                var customer = Customers.First(c => c.Id == id);

                var user = await _userManager.FindByIdAsync(customer.ApplicationUserFk);

                // unblock
                await _userManager.UnblockUser(user);

                _logger.LogInformation($"Unblocked user with id: {user.Id}");
                StatusMessage = _localizer["User unblocked successfully"];
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Could not find customer with id {id}: {e.Message}");
            }


            return Page();
        }

        public async Task<IActionResult> OnPostDeleteCustomerAsync(int id)
        {
            try
            {
                var customer = Customers.First(c => c.Id == id);

                var user = await _userManager.FindByIdAsync(customer.ApplicationUserFk);

                await _userManager.DeleteAsync(user);

                await _mediator.Send(new DeleteCustomerCommand
                {
                    Customer = customer
                });

                _logger.LogInformation($"Deleted user with id: {user.Id}");
                StatusMessage = _localizer["User deleted successfully"];
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Could not find customer with id {id}: {e.Message}");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostBlockRiderAsync(int id)
        {
            try
            {
                var rider = Riders.First(r => r.Id == id);
                var user = await _userManager.FindByIdAsync(rider.Customer.ApplicationUserFk);

                // block the user
                await _userManager.BlockUser(user);

                _logger.LogInformation($"Blocked user with id: {user.Id}");
                StatusMessage = _localizer["User blocked successfully"];
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Could not find rider with id {id}: {e.Message}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUnblockRiderAsync(int id)
        {
            try
            {
                var rider = Riders.First(c => c.Id == id);

                var user = await _userManager.FindByIdAsync(rider.Customer.ApplicationUserFk);

                // unblock
                await _userManager.UnblockUser(user);

                _logger.LogInformation($"Unblocked user with id: {user.Id}");
                StatusMessage = _localizer["User unblocked successfully"];
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Could not find rider with id {id}: {e.Message}");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteRiderAsync(int id)
        {
            try
            {
                var rider = Riders.First(r => r.Id == id);
                var user = await _userManager.FindByIdAsync(rider.Customer.ApplicationUserFk);


                await _mediator.Send(new DeleteRiderCommand
                {
                    Id = rider.Id
                });
                await _userManager.DeleteAsync(user);

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Deleted user with id: {user.Id}");
                StatusMessage = _localizer["User deleted successfully"];
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Could not find rider with id {id}: {e.Message}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostBlockRestaurateurAsync(int id)
        {
            try
            {
                var restaurateur = Restaurateurs.First(r => r.Id == id);
                var user = await _userManager.FindByIdAsync(restaurateur.Customer.ApplicationUserFk);

                // block the user
                await _userManager.BlockUser(user);

                _logger.LogInformation($"Blocked user with id: {user.Id}");
                StatusMessage = _localizer["User blocked successfully"];
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Could not find restaurateur with id {id}: {e.Message}");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUnblockRestaurateurAsync(int id)
        {
            try
            {
                var restaurateur = Restaurateurs.First(c => c.Id == id);

                var user = await _userManager.FindByIdAsync(restaurateur.Customer.ApplicationUserFk);

                // unblock
                await _userManager.UnblockUser(user);

                _logger.LogInformation($"Unblocked user with id: {user.Id}");
                StatusMessage = _localizer["User unblocked successfully"];
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Could not find restaurateur with id {id}: {e.Message}");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteRestaurateurAsync(int id)
        {
            try
            {
                var restaurateur = Restaurateurs.First(r => r.Id == id);
                var user = await _userManager.FindByIdAsync(restaurateur.Customer.ApplicationUserFk);

                await _mediator.Send(new DeleteRestaurateurCommand
                {
                    Id = restaurateur.Id
                });
                await _userManager.DeleteAsync(user);

                _logger.LogInformation($"Deleted user with id: {user.Id}");
                StatusMessage = _localizer["User deleted successfully"];
            }
            catch (InvalidOperationException e)
            {
                _logger.LogError($"Could not find restaurateur with id {id}: {e.Message}");
            }

            return Page();
        }

        public async Task<ApplicationUser> GetUserAsync(int id, string listName)
        {
            switch (listName)
            {
                case nameof(Customers):
                    var customer = Customers.First(u => u.Id == id);
                    return await _userManager.FindByIdAsync(customer.ApplicationUserFk);

                case nameof(Riders):
                    var rider = Riders.First(u => u.Id == id);
                    return await _userManager.FindByIdAsync(rider.Customer.ApplicationUserFk);

                case nameof(Restaurateurs):
                    var restaurateur = Restaurateurs.First(u => u.Id == id);
                    return await _userManager.FindByIdAsync(restaurateur.Customer.ApplicationUserFk);
            }

            return null;
        }

    }
}
