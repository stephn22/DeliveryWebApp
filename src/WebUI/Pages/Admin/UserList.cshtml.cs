using DeliveryWebApp.Application.Clients.Queries;
using DeliveryWebApp.Application.Clients.Queries.GetClients;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Restaurateurs.Queries.GetRestaurateurs;
using DeliveryWebApp.Application.Riders.Queries.GetRiders;

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

        public IList<Client> Clients { get; set; }
        public IList<Rider> Riders { get; set; }
        public IList<Domain.Entities.Restaurateur> Restaurateurs { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            Clients = await _mediator.Send(new GetClientsQuery());
            Riders = await _mediator.Send(new GetRidersQuery());
            Restaurateurs = await _mediator.Send(new GetRestaurateursQuery());

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return Page();
        }

        private async Task<ApplicationUser> GetUserAsync(int id, string listName)
        {
            switch (listName)
            {
                case nameof(Clients):
                    var client = Clients.First(u => u.Id == id);
                    return await _userManager.FindByIdAsync(client.ApplicationUserFk);

                case nameof(Riders):
                    var rider = Riders.First(u => u.Id == id);
                    return await _userManager.FindByIdAsync(rider.Client.ApplicationUserFk);

                case nameof(Restaurateurs):
                    var restaurateur = Restaurateurs.First(u => u.Id == id);
                    return await _userManager.FindByIdAsync(restaurateur.Client.ApplicationUserFk);
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

        public async Task<string> GetEmailAsync(int id, string listname)
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
