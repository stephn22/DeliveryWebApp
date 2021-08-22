using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommand : IRequest<Address>
    {
        public int Id { get; set; }
        public string PlaceName { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
    }

    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Address>
    {
        private readonly IApplicationDbContext _context;

        public UpdateAddressCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Addresses.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Address), request.Id);
            }

            entity.PlaceName = request.PlaceName;

            entity.Latitude = request.Latitude;
            entity.Longitude = request.Longitude;

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}