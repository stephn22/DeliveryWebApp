using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
using DeliveryWebApp.Infrastructure.Security;
using DeliveryWebApp.Infrastructure.Services.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
        public string CurrentRole { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {

            [Required]
            [DataType(DataType.Currency)]
            public decimal DeliveryCredit { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // get customer by id
            Customer = await _context.Customers.FindAsync(id);

            var user = await _userManager.FindByIdAsync(Customer.ApplicationUserFk);

            CurrentRole = await _userManager.GetRoleAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostBlockCustomerAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Customer = await _context.Customers.FindAsync(id);

            var user = await _userManager.FindByIdAsync(Customer.ApplicationUserFk);

            await _userManager.BlockUser(user);

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostUnblockCustomerAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Customer = await _context.Customers.FindAsync(id);

            var user = await _userManager.FindByIdAsync(Customer.ApplicationUserFk);

            await _userManager.UnblockUser(user);

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostDeleteUserAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Customer = await _context.Customers.FindAsync(id);

            var user = await _userManager.FindByIdAsync(Customer.ApplicationUserFk);

            await _userManager.DeleteAsync(user);

            await _mediator.Send(new DeleteCustomerCommand
            {
                Id = Customer.Id
            });

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostToCustomerAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Customer = await _context.Customers.FindAsync(id);

            var user = await _userManager.FindByIdAsync(Customer.ApplicationUserFk);
            CurrentRole = await _userManager.GetRoleAsync(user);

            if (CurrentRole != RoleName.Default)
            {
                // update claim
                var oldClaim = await _userManager.GetClaimByTypeAsync(user, ClaimName.Role);
                await _userManager.ReplaceClaimAsync(user, oldClaim, new Claim(ClaimName.Role, RoleName.Default));

                // update tables TODO: check if cascade
                switch (CurrentRole)
                {
                    case RoleName.Rider: // remove from table if rider
                        var rider = await _context.GetRiderByCustomerIdAsync(id);

                        await _mediator.Send(new DeleteRiderCommand
                        {
                            Id = rider.Id
                        });
                        break;

                    case RoleName.Restaurateur: // remove from table if restaurateur
                        var restaurateur = await _context.GetRestaurateurByCustomerIdAsync(id);

                        await _mediator.Send(new DeleteRestaurateurCommand
                        {
                            Id = restaurateur.Id
                        });
                        break;
                }
            }

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostToRiderAsync(int? id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Customer = await _context.Customers.FindAsync(id);

            var user = await _userManager.FindByIdAsync(Customer.ApplicationUserFk);
            CurrentRole = await _userManager.GetRoleAsync(user);

            if (CurrentRole != RoleName.Rider)
            {
                var oldClaim = await _userManager.GetClaimByTypeAsync(user, ClaimName.Role);

                await _userManager.ReplaceClaimAsync(user, oldClaim, new Claim(ClaimName.Role, RoleName.Rider));

                try // if user was a restaurateur delete from table
                {
                    var restaurateur = await _context.Restaurateurs.Where(r => r.Customer.Id == Customer.Id).FirstAsync();

                    await _mediator.Send(new DeleteRestaurateurCommand
                    {
                        Id = restaurateur.Id
                    });
                }
                catch (InvalidOperationException)
                {
                    // otherwise do nothing
                }

                // update table

                await _mediator.Send(new CreateRiderCommand
                {
                    Customer = Customer,
                    DeliveryCredit = Input.DeliveryCredit
                });
            }
            else // update only delivery credit
            {
                var rider = await _context.GetRiderByCustomerIdAsync(id);

                await _mediator.Send(new UpdateRiderCommand
                {
                    Id = rider.Id,
                    DeliveryCredit = Input.DeliveryCredit
                });
            }

            return RedirectToPage("/AdminPages/UserList");
        }

        public async Task<IActionResult> OnPostToRestaurateurAsync(int? id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Customer = await _context.Customers.FindAsync(id);

            var user = await _userManager.FindByIdAsync(Customer.ApplicationUserFk);
            CurrentRole = await _userManager.GetRoleAsync(user);

            if (CurrentRole != RoleName.Restaurateur)
            {
                var oldClaim = await _userManager.GetClaimByTypeAsync(user, ClaimName.Role);
                await _userManager.ReplaceClaimAsync(user, oldClaim, new Claim(ClaimName.Role, RoleName.Restaurateur));

                // update tables

                try // if user was a rider, delete from table
                {
                    var rider = await _context.Riders.Where(r => r.CustomerId == Customer.Id).FirstAsync();

                    await _mediator.Send(new DeleteRiderCommand
                    {
                        Id = rider.Id
                    });
                }
                catch (InvalidOperationException)
                {
                    // otherwise do nothing
                }
                
                await _mediator.Send(new CreateRestaurateurCommand
                {
                    Customer = Customer
                });
            }
            return RedirectToPage("/AdminPages/UserList");
        }
    }
}