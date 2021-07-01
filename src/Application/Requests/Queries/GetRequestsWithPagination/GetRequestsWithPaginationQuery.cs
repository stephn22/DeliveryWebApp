using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Common.Mappings;
using DeliveryWebApp.Application.Common.Models;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Requests.Queries.GetRequestsWithPagination
{
    public class GetRequestsWithPaginationQuery : IRequest<PaginatedList<Request>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class
        GetRequestsWithPaginationQueryHandler : IRequestHandler<GetRequestsWithPaginationQuery, PaginatedList<Request>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetRequestsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<Request>> Handle(GetRequestsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.Requests
                .OrderBy(r => r.Id)
                .ProjectTo<Request>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
