using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Restaurants.Queries.GetRestaurants
{
    public class GetRestaurantsQuery : IRequest<List<Restaurant>>
    {
    }

    public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, List<Restaurant>>
    {
        private readonly IApplicationDbContext _context;

        public GetRestaurantsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Restaurant>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Restaurants.ToListAsync(cancellationToken);
        }
    }
}
