using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Application.Clients.Queries
{
    public class GetRestaurateursQuery : IRequest<List<Restaurateur>>
    {
    }

    public class GetRestaurateursQueryHandler : IRequestHandler<GetRestaurateursQuery, List<Restaurateur>>
    {
        private readonly IApplicationDbContext _context;

        public GetRestaurateursQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Restaurateur>> Handle(GetRestaurateursQuery request, CancellationToken cancellationToken)
        {
            return await _context.Restaurateurs.ToListAsync(cancellationToken);
        }
    }
}
