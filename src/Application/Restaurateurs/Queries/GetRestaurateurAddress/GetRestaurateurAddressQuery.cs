using System;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.Application.Restaurateurs.Queries.GetRestaurateurAddress
{
    public class GetRestaurateurAddressQuery : IRequest<Address>
    {
        /// <summary>
        /// Restaurateur id
        /// </summary>
        public int Id { get; set; }
    }

    public class GetRestaurateurAddressQueryHandler : IRequestHandler<GetRestaurateurAddressQuery, Address>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<GetRestaurateurAddressQuery> _logger;

        public GetRestaurateurAddressQueryHandler(IApplicationDbContext context, ILogger<GetRestaurateurAddressQuery> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Address> Handle(GetRestaurateurAddressQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var r = await _context.Restaurateurs.FindAsync(request.Id);
                return r.RestaurantAddress;
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning($"{nameof(Restaurateur)}, {e.Message}");
                return null;
            }
        }
    }
}
