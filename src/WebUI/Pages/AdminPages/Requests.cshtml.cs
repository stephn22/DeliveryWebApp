using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.AdminPages
{
    [Authorize(Roles = RoleName.Admin)]
    public class RequestsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RequestsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Request> Requests { get; set; }

        public async Task OnGetAsync()
        {
            var requests = from r in _context.Requests
                           where r.Status == RequestStatus.Idle
                           select r;

            Requests = await requests.ToListAsync();
        }
    }
}
