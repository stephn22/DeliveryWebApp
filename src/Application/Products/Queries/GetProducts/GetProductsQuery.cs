using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<List<Product>>
    {
        public int? RestaurateurId { get; set; }
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
                if (request.RestaurateurId != null)
                {
                    return await _context.Products.Where(p => p.RestaurateurId == request.RestaurateurId).ToListAsync(cancellationToken);
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
