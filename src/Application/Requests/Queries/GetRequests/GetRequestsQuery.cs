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

namespace DeliveryWebApp.Application.Requests.Queries.GetRequests
{
    public class GetRequestsQuery : IRequest<List<Request>>
    {
    }

    public class GetRequestsQueryHandler : IRequestHandler<GetRequestsQuery, List<Request>>
    {
        private readonly IApplicationDbContext _context;

        public GetRequestsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Request>> Handle(GetRequestsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Requests.ToListAsync(cancellationToken);
        }
    }
}
