using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Customers.Commands.DeleteCustomer;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Application.Restaurateurs.Commands.DeleteRestaurateur;
using DeliveryWebApp.Application.Restaurateurs.Extensions;
using DeliveryWebApp.Application.Riders.Commands.CreateRider;
using DeliveryWebApp.Application.Riders.Commands.DeleteRider;
using DeliveryWebApp.Application.Riders.Commands.UpdateRider;
using DeliveryWebApp.Application.Riders.Extensions;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.AdminPages
{
    [Authorize(Roles = RoleName.Admin)]
    public class CustomerDetailModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CustomerDetailModel> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public CustomerDetailModel(ApplicationDbContext context, ILogger<CustomerDetailModel> logger,
            UserManager<ApplicationUser> userManager, IMediator mediator)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _mediator = mediator;
        }

        public Customer Customer { get; set; }
        public ApplicationUser User { get; set; }
        public string CurrentRole { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {

            [Required]
            [DataType(DataType.Currency)]
            [DisplayName("Delivery Credit")]
            public decimal DeliveryCredit { get; set; }
        }

        private async Task LoadAsync(int id)
        {
            // get customer by id
            Customer = await _context.Customers.FindAsync(id);

            User = await _userManager.FindByIdAsync(Customer.ApplicationUserFk);

            CurrentRole = await _userManager.GetRoleAsync(User);
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

            if (Customer == null)
            {
                return NotFound("Unable to load entities");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostBlockCustomerAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

            await _userManager.BlockUser(User);

            _logger.LogInformation($"Blocked user with id '{User.Id}");

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostUnblockCustomerAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

            await _userManager.UnblockUser(User);

            _logger.LogInformation($"Unblocked user with id '{User.Id}");

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }


            await LoadAsync((int)id);

            await _userManager.DeleteAsync(User);

            await _mediator.Send(new DeleteCustomerCommand
            {
                Customer = Customer
            });

            _logger.LogInformation($"Deleted customer with id '{Customer.Id}");

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostToCustomerAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

            // update claim
            var oldClaim = await _userManager.GetClaimByTypeAsync(User, ClaimName.Role);
            await _userManager.ReplaceClaimAsync(User, oldClaim, new Claim(ClaimName.Role, RoleName.Default));

            // update tables
            switch (CurrentRole)
            {
                case RoleName.Rider: // remove from table if rider
                    var rider = await _context.GetRiderByCustomerIdAsync(id);

                    await _mediator.Send(new DeleteRiderCommand
                    {
                        Id = rider.Id
                    });

                    _logger.LogInformation($"Deleted rider with id {rider.Id}");

                    break;

                case RoleName.Restaurateur: // remove from table if restaurateur
                    var restaurateur = await _context.GetRestaurateurByCustomerIdAsync(id);

                    await _mediator.Send(new DeleteRestaurateurCommand
                    {
                        Id = restaurateur.Id
                    });

                    _logger.LogInformation($"Deleted restaurateur with id {restaurateur.Id}");

                    break;
            }

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostToRiderAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }


            await LoadAsync((int)id);

            if (CurrentRole != RoleName.Rider)
            {
                var oldClaim = await _userManager.GetClaimByTypeAsync(User, ClaimName.Role);

                await _userManager.ReplaceClaimAsync(User, oldClaim, new Claim(ClaimName.Role, RoleName.Rider));

                try // if user was a restaurateur delete from table
                {
                    var restaurateur = await _context.Restaurateurs.Where(r => r.Customer.Id == Customer.Id).FirstAsync();

                    await _mediator.Send(new DeleteRestaurateurCommand
                    {
                        Id = restaurateur.Id
                    });

                    _logger.LogInformation($"Deleted restaurateur with id {restaurateur.Id}");
                }
                catch (InvalidOperationException)
                {
                    // otherwise do nothing
                }

                // update table

                var r = await _mediator.Send(new CreateRiderCommand
                {
                    Customer = Customer,
                    DeliveryCredit = Input.DeliveryCredit
                });

                _logger.LogInformation($"Created rider with id {r.Id}");
            }
            else // update only delivery credit (is a rider already)
            {
                var rider = await _context.GetRiderByCustomerIdAsync(id);

                await _mediator.Send(new UpdateRiderCommand
                {
                    Id = rider.Id,
                    DeliveryCredit = Input.DeliveryCredit
                });

                _logger.LogInformation($"Updated rider with id {rider.Id}");
            }

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostToRestaurateurAsync(int? id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (id == null)
            {
                return NotFound();
            }

            await LoadAsync((int)id);

            var oldClaim = await _userManager.GetClaimByTypeAsync(User, ClaimName.Role);
            await _userManager.ReplaceClaimAsync(User, oldClaim, new Claim(ClaimName.Role, RoleName.Restaurateur));

            // update tables

            try // if user was a rider, delete from table
            {
                var rider = await _context.Riders.Where(r => r.CustomerId == Customer.Id).FirstAsync();

                await _mediator.Send(new DeleteRiderCommand
                {
                    Id = rider.Id
                });

                _logger.LogInformation($"Deleted rider with id {rider.Id}");
            }
            catch (InvalidOperationException)
            {
                // otherwise do nothing
            }

            var r = await _mediator.Send(new CreateRestaurateurCommand
            {
                Customer = Customer
            });

            _logger.LogInformation($"Created restaurateur with id {r.Id}");

            return RedirectToPage("/AdminPages/UserList");
        }
    }
}