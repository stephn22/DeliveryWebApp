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

namespace DeliveryWebApp.Application.Customers.Queries.GetCustomersWithPagination;

public class GetCustomersWithPaginationQuery : IRequest<PaginatedList<Customer>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class
    GetCustomersWithPaginationQueryHandler : IRequestHandler<GetCustomersWithPaginationQuery,
        PaginatedList<Customer>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCustomersWithPaginationQuery> _logger;

    public GetCustomersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper, ILogger<GetCustomersWithPaginationQuery> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PaginatedList<Customer>> Handle(GetCustomersWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _context.Customers
                .OrderBy(c => c.Id)
                .ProjectTo<Customer>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning($"{nameof(Customer)}, {e.Message}");
            return null;
        }
    }
}