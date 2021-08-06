using System;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public GetRestaurateurAddressQueryHandler(IApplicationDbContext context,
            ILogger<GetRestaurateurAddressQuery> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Address> Handle(GetRestaurateurAddressQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var entity =
                    await _context.Addresses.FirstAsync(a => a.RestaurateurId == request.Id, cancellationToken);
                return entity;
            }
            catch (InvalidOperationException e)
            {
                _logger.LogWarning($"{nameof(Restaurateur)}, {e.Message}");
                return null;
            }
        }
    }
}