using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Common.Mappings;
using DeliveryWebApp.Application.Common.Models;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Requests.Queries.GetRequestsWithPagination;

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
    private readonly ILogger<GetRequestsWithPaginationQuery> _logger;

    public GetRequestsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, ILogger<GetRequestsWithPaginationQuery> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedList<Request>> Handle(GetRequestsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _context.Requests
                .OrderBy(r => r.Id)
                .ProjectTo<Request>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning($"{nameof(Request)}, {e.Message}");
            return null;
        }
    }
}