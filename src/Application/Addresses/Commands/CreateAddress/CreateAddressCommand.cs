using AutoMapper;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Addresses.Commands.CreateAddress
{
    public class CreateAddressCommand : IRequest<Address>
    {
        public string PlaceName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int? CustomerId { get; set; }
        public int? RestaurateurId { get; set; }
    }

    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Address>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateAddressCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Address> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Address>(request);

            if (request.CustomerId != null)
            {
                entity.CustomerId = request.CustomerId;
            }

            if (request.RestaurateurId != null)
            {
                entity.RestaurateurId = request.RestaurateurId;
            }

            if (!string.IsNullOrEmpty(request.PlaceName)) // if placename isn't provided use other properties
            {
                var line = $"{request.AddressLine1}, " +
                           $"{request.AddressLine2}, " +
                           $"{request.Number}, " +
                           $"{request.City}, " +
                           $"{request.StateProvince}, " +
                           $"{request.PostalCode}, " +
                           $"{request.Country}";

                entity.PlaceName = line;
            }

            entity.PlaceName = request.PlaceName;

            _context.Addresses.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
