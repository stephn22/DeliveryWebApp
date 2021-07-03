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
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.Application.Requests.Queries.GetRequests
{
    public class GetRequestsQuery : IRequest<List<Request>>
    {
    }

    public class GetRequestsQueryHandler : IRequestHandler<GetRequestsQuery, List<Request>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetRequestsQuery> _logger;

        public GetRequestsQueryHandler(IApplicationDbContext context, ILogger<GetRequestsQuery> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Request>> Handle(GetRequestsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Requests.ToListAsync(cancellationToken);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning($"{nameof(Request)}, {e.Message}");
                return null;
            }
        }
    }
}
