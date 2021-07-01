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

namespace DeliveryWebApp.Application.Orders.Queries.GetOrders
{
    public class GetOrdersQuery : IRequest<List<Order>>
    {
        public int? CustomerId { get; set; }
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
            if (request.CustomerId == null) // for administrator
            {
                return await _context.Orders.ToListAsync(cancellationToken);
            }

            // for customer
            return await (from o in _context.Orders
                where o.CustomerId == request.CustomerId
                select o).ToListAsync(cancellationToken);
        }
    }
}
