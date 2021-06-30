using DeliveryWebApp.Application.Restaurateurs.Queries.GetRestaurateurs;
using DeliveryWebApp.Application.Riders.Queries.GetRiders;
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
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Customers.Commands.DeleteCustomer;
using DeliveryWebApp.Application.Customers.Queries.GetCustomers;

namespace DeliveryWebApp.WebUI.Pages.Admin
{
    [Authorize(Roles = RoleName.Admin)]
    public class UserListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserListModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public UserListModel(ApplicationDbContext context, ILogger<UserListModel> logger, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _logger = logger;
            _mediator = mediator;
            _userManager = userManager;
        }

        public IList<Domain.Entities.Customer> Customers { get; set; }
        public IList<Rider> Riders { get; set; }
        public IList<Domain.Entities.Restaurateur> Restaurateurs { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Customers = await _mediator.Send(new GetCustomersQuery());
            Riders = await _mediator.Send(new GetRidersQuery());
            Restaurateurs = await _mediator.Send(new GetRestaurateursQuery());

            return Page();
        }

        public async Task<IActionResult> OnPostBlockCustomerAsync(int id)
        {
            Customers = await _mediator.Send(new GetCustomersQuery());

            var customer = Customers.First(c => c.Id == id);

            var user = await _userManager.FindByIdAsync(customer.ApplicationUserFk);

            // block the user
            await _userManager.BlockUser(user);

            _logger.LogInformation($"Blocked user with id: {user.Id}");

            return Page();
        }

        public async Task<IActionResult> OnPostUnblockCustomerAsync(int id)
        {
            Customers = await _mediator.Send(new GetCustomersQuery());

            var customer = Customers.First(c => c.Id == id);

            var user = await _userManager.FindByIdAsync(customer.ApplicationUserFk);

            // unblock
            await _userManager.UnblockUser(user);

            _logger.LogInformation($"Blocked user with id: {user.Id}");

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteCustomerAsync(int id)
        {
            Customers = await _mediator.Send(new GetCustomersQuery());

            var customer = Customers.First(c => c.Id == id);

            var user = await _userManager.FindByIdAsync(customer.ApplicationUserFk);

            await _userManager.DeleteAsync(user);

            await _mediator.Send(new DeleteCustomerCommand
            {
                Id = id
            });

            _logger.LogInformation($"Deleted user with id: {user.Id}");

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostBlockRiderAsync(int id)
        {
            Riders = await _mediator.Send(new GetRidersQuery());

            var rider = Riders.First(r => r.Id == id);
            var user = await _userManager.FindByIdAsync(rider.Customer.ApplicationUserFk);

            // block the user
            await _userManager.BlockUser(user);

            _logger.LogInformation($"Blocked user with id: {user.Id}");

            return Page();
        }

        public async Task<IActionResult> OnPostUnblockRiderAsync(int id)
        {
            Riders = await _mediator.Send(new GetRidersQuery());

            var rider = Riders.First(c => c.Id == id);

            var user = await _userManager.FindByIdAsync(rider.Customer.ApplicationUserFk);

            // unblock
            await _userManager.UnblockUser(user);

            _logger.LogInformation($"Blocked user with id: {user.Id}");

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteRiderAsync(int id)
        {
            Riders = await _mediator.Send(new GetRidersQuery());

            var rider = Riders.First(r => r.Id == id);
            var user = await _userManager.FindByIdAsync(rider.Customer.ApplicationUserFk);

            await _userManager.DeleteAsync(user);
            _context.Riders.Remove(rider);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Deleted user with id: {user.Id}");

            return Page();
        }

        public async Task<IActionResult> OnPostBlockRestaurateurAsync(int id)
        {
            Restaurateurs = await _mediator.Send(new GetRestaurateursQuery());

            var restaurateur = Restaurateurs.First(r => r.Id == id);
            var user = await _userManager.FindByIdAsync(restaurateur.Customer.ApplicationUserFk);

            // block the user
            await _userManager.BlockUser(user);

            _logger.LogInformation($"Blocked user with id: {user.Id}");

            return Page();
        }

        public async Task<IActionResult> OnPostUnblockRestaurateurAsync(int id)
        {
            Restaurateurs = await _mediator.Send(new GetRestaurateursQuery());

            var restaurateur = Restaurateurs.First(c => c.Id == id);

            var user = await _userManager.FindByIdAsync(restaurateur.Customer.ApplicationUserFk);

            // unblock
            await _userManager.UnblockUser(user);

            _logger.LogInformation($"Blocked user with id: {user.Id}");

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteRestaurateurAsync(int id)
        {
            Restaurateurs = await _mediator.Send(new GetRestaurateursQuery());

            var restaurateur = Restaurateurs.First(r => r.Id == id);
            var user = await _userManager.FindByIdAsync(restaurateur.Customer.ApplicationUserFk);

            await _userManager.DeleteAsync(user);
            _context.Restaurateurs.Remove(restaurateur);

            _logger.LogInformation($"Deleted user with id: {user.Id}");

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

        public async Task<string> GetFNameAsync(int id, string listName)
        {
            var user = await GetUserAsync(id, listName);

            if (user == null)
            {
                return "";
            }

            var fName = (from claim in await _userManager.GetClaimsAsync(user)
                         where claim.Type == ClaimName.FName
                         select claim.Value).First();

            return fName;
        }

        public async Task<string> GetLNameAsync(int id, string listName)
        {
            var user = await GetUserAsync(id, listName);

            if (user == null)
            {
                return "";
            }

            var lName = (from claim in await _userManager.GetClaimsAsync(user)
                         where claim.Type == ClaimName.LName
                         select claim.Value).First();

            return lName;
        }

        public async Task<string> GetEmailByIdAsync(int id, string listname)
        {
            var user = await GetUserAsync(id, listname);

            if (user == null)
            {
                return "";
            }

            return await _userManager.GetEmailAsync(user);
        }
    }
}
