using System;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<List<Product>>
    {
        public int? RestaurantId { get; set; }
        public int? OrderId { get; set; }
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<Product>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetProductsQuery> _logger;

        public GetProductsQueryHandler(IApplicationDbContext context, ILogger<GetProductsQuery> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.RestaurantId != null)
                {
                    return await (from r in _context.Restaurants
                        where r.Id == request.RestaurantId
                        select r.Products.ToList()).FirstAsync(cancellationToken);
                }

                if (request.OrderId != null)
                {
                    return await (from o in _context.Orders
                        where o.Id == request.OrderId
                        select o.Products.ToList()).FirstAsync(cancellationToken);
                }

                return null;
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning($"{nameof(Product)}, {e.Message}");
                return null;
            }
        }
    }
}
