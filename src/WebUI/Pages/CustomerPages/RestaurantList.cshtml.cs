using DeliveryWebApp.Application.Addresses.Queries.GetAddresses;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.CustomerPages
{
    public class RestaurantListModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;

        public RestaurantListModel(ApplicationDbContext context, IMediator mediator, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mediator = mediator;
            _userManager = userManager;
        }

        public List<Restaurant> Restaurants { get; set; }
        public Customer Customer { get; set; }
        public List<Address> CustomerAddresses { get; set; }

        public string NameSort { get; set; }
        public string CategorySort { get; set; }
        public string DistanceSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task<IActionResult> OnGetAsync(string sortOrder)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Customer = await _context.Customers.Where(c => c.ApplicationUserFk == user.Id).FirstAsync();
            //Restaurants = await _mediator.Send(new GetRestaurantsQuery());
            CustomerAddresses = await _mediator.Send(new GetAddressesQuery
            {
                CustomerId = Customer.Id
            });

            var restaurantsOrd = from r in _context.Restaurants
                                 select r;

            NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            CategorySort = string.IsNullOrEmpty(sortOrder) ? "category_desc" : "";
            //TODO: Distance?

            restaurantsOrd = sortOrder switch
            {
                "name_desc" => restaurantsOrd.OrderByDescending(r => r.Name),
                "category_desc" => restaurantsOrd.OrderByDescending(r => r.Category),
                _ => restaurantsOrd.OrderBy(r => r.Name)
            };

            Restaurants = await restaurantsOrd.AsNoTracking().ToListAsync();

            return Page();
        }
    }
}