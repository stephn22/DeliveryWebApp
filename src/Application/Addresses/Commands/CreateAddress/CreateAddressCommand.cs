using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Addresses.Commands.CreateAddress
{
    public class CreateAddressCommand : IRequest<Address>
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string StateProvince { get; set; }
        public string Country { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public Customer Customer { get; set; }
        public Restaurateur Restaurateur { get; set; }
    }

    public class CreateAddressCommandHandler : IRequestHandler<CreateAddressCommand, Address>
    {
        private readonly IApplicationDbContext _context;

        public CreateAddressCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address> Handle(CreateAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = new Address
            {
                AddressLine1 = request.AddressLine1,
                AddressLine2 = request.AddressLine2,
                City = request.City,
                StateProvince = request.StateProvince,
                Country = request.Country,
                Number = request.Number,
                PostalCode = request.PostalCode,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
            };

            if (request.Customer != null)
            {
                entity.CustomerId = request.Customer.Id;
            }

            if (request.Restaurateur != null)
            {
                entity.RestaurateurId = request.Restaurateur.Id;
            }

            _context.Addresses.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
