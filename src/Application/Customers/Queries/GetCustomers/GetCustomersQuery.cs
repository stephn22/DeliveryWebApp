using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Customers.Queries.GetCustomers;

public class GetCustomersQuery : IRequest<List<Customer>>
{
}

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<Customer>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetCustomersQuery> _logger;

    public GetCustomersQueryHandler(IApplicationDbContext context, ILogger<GetCustomersQuery> logger)
    {
        _context = context;
        _logger = logger;

    }

    public async Task<List<Customer>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _context.Customers.ToListAsync(cancellationToken);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning($"{nameof(Customer)}, {e.Message}");
            return null;
        }
    }
}