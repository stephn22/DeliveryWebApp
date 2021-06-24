using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Application.Riders.Queries.GetRiders
{
    public class GetRidersQuery : IRequest<List<Rider>>
    {
    }

    public class GetRidersQueryHandler : IRequestHandler<GetRidersQuery, List<Rider>>
    {
        private readonly IApplicationDbContext _context;

        public GetRidersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rider>> Handle(GetRidersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Riders.ToListAsync(cancellationToken);
        }
    }
}
