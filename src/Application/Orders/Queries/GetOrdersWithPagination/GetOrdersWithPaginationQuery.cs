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

namespace DeliveryWebApp.Application.Orders.Queries.GetOrdersWithPagination;

public class GetOrdersWithPaginationQuery : IRequest<PaginatedList<Order>>
{
    public int? CustomerId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class
    GetOrdersWithPaginationQueryHandler : IRequestHandler<GetOrdersWithPaginationQuery, PaginatedList<Order>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetOrdersWithPaginationQuery> _logger;

    public GetOrdersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, ILogger<GetOrdersWithPaginationQuery> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedList<Order>> Handle(GetOrdersWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.CustomerId == null)
            {
                return await _context.Orders
                    .OrderBy(o => o.Date)
                    .ProjectTo<Order>(_mapper.ConfigurationProvider)
                    .PaginatedListAsync(request.PageNumber, request.PageSize);
            }

            return await _context.Orders
                .Where(c => c.CustomerId == request.CustomerId)
                .OrderBy(o => o.Date)
                .ProjectTo<Order>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning($"{nameof(Order)}, {e.Message}");
            return null;
        }
    }
}