using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Orders.Queries.GetOrders;

public class GetOrdersQuery : IRequest<List<Order>>
{
    public int? CustomerId { get; set; }
    public int? RestaurateurId { get; set; }
}

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<Order>>
{
    private readonly IApplicationDbContext _context;

    public GetOrdersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.CustomerId == null && request.RestaurateurId == null) // for administrator and rider
            {
                return await _context.Orders.ToListAsync(cancellationToken);
            }

            if (request.RestaurateurId != null) // for restaurateur
            {
                return await _context.Orders.Where(o => o.RestaurateurId == request.RestaurateurId)
                    .ToListAsync(cancellationToken);
            }

            // for customer
            return await (_context.Orders.Where(o => o.CustomerId == request.CustomerId)).ToListAsync(cancellationToken);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
}