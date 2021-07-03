using System;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.Application.Restaurants.Queries.GetRestaurants
{
    public class GetRestaurantsQuery : IRequest<List<Restaurant>>
    {
    }

    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, List<Restaurant>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetRestaurantsQuery> _logger;

        public GetRestaurantsQueryHandler(IApplicationDbContext context, ILogger<GetRestaurantsQuery> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Restaurant>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Restaurants.ToListAsync(cancellationToken);
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning($"{nameof(Restaurant)}, {e.Message}");
                return null;
            }
        }
    }
}
