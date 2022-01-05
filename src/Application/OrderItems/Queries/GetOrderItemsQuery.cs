using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.OrderItems.Queries
{
    public class GetOrderItemsQuery : IRequest<List<OrderItem>>
    {
        public int OrderId { get; set; }
    }

    public class GetOrderItemsQueryHandler : IRequestHandler<GetOrderItemsQuery, List<OrderItem>>
    {
        private readonly IApplicationDbContext _context;

        public GetOrderItemsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderItem>> Handle(GetOrderItemsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await _context.OrderItems.Where(o => o.OrderId == request.OrderId)
                    .ToListAsync(cancellationToken);

                return list;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
