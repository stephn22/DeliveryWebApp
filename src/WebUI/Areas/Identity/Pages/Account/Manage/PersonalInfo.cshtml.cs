using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Customers.Commands.UpdateCustomer;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.WebUI.Areas.Identity.Pages.Account.Manage
{
    // FIXME: Not working
    public class PersonalInfoModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        private readonly SignInManager<ApplicationUser> _signInManager;
        // TODO add _logger

        public PersonalInfoModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager,
            IMediator mediator, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _mediator = mediator;
            _signInManager = signInManager;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [BindProperty] public InputModel Input { get; set; }

        public class InputModel
        {
            [DataType(DataType.Text)]
            [DisplayName("New First Name")]
            public string FName { get; set; }

            [DataType(DataType.Text)]
            [DisplayName("New Last Name")]
            public string LName { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var customer = await _context.Customers.Where(c => c.ApplicationUserFk == user.Id).FirstAsync();

            FirstName = customer.FirstName;
            LastName = customer.LastName;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);


            if (user == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var customer = await _context.Customers.Where(c => c.ApplicationUserFk == user.Id).FirstAsync();

            if (Input.FName != customer.FirstName)
            {
                await _mediator.Send(new UpdateCustomerCommand
                {
                    Id = customer.Id,
                    Fname = Input.FName
                });
            }

            if (Input.LName != customer.LastName)
            {
                await _mediator.Send(new UpdateCustomerCommand
                {
                    Id = customer.Id,
                    LName = Input.LName
                });
            }

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToPage();
        }
        
    }
}
