using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Riders.Queries.GetRiders
{
    public class GetRidersQuery : IRequest<List<Rider>>
    {
    }

    public class GetRidersQueryHandler : IRequestHandler<GetRidersQuery, List<Rider>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetRidersQuery> _logger;

        public GetRidersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Rider>> Handle(GetRidersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Riders.ToListAsync(cancellationToken);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning($"{nameof(Rider)}, {e.Message}");
                return null;
            }
        }
    }
}
