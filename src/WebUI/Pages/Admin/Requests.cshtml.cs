using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence;
using DeliveryWebApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.WebUI.Pages.Admin
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
