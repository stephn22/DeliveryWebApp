using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Restaurants.Queries.GetAddress
{
    public class GetRestaurantAddressQuery : IRequest<Address>
    {
        public int AddressId { get; set; }
    }

    public class GetRestaurantAddressQueryHandler : IRequestHandler<GetRestaurantAddressQuery, Address>
    {
        private readonly IApplicationDbContext _context;

        public GetRestaurantAddressQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address> Handle(GetRestaurantAddressQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Addresses.FindAsync(request.AddressId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Address), request.AddressId);
            }

            return entity;
        }
    }
}
