using DeliveryWebApp.Application.Common.Models;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryWebApp.WebUI.Pages.AdminPages
{
    [Authorize(Roles = RoleName.Admin)]
    public class RequestsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public RequestsModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public PaginatedList<Request> Requests { get; set; }

        public async Task OnGetAsync(int? pageIndex)
        {
            var requests = _context.Requests.Where(r => r.Status == RequestStatus.Idle);

            var pageSize = _configuration.GetValue("PageSize", 5);
            Requests = await PaginatedList<Request>.CreateAsync(requests.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}
