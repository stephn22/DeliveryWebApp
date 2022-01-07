using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Addresses.Commands.CreateAddress;

public class CreateAddressCommand : IRequest<Address>
{
    public string PlaceName { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public int? CustomerId { get; set; }
    public int? RestaurateurId { get; set; }
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
            Longitude = request.Longitude,
            Latitude = request.Latitude,
            PlaceName = request.PlaceName,
            CustomerId = request.CustomerId,
            RestaurateurId = request.RestaurateurId
        };

        _context.Addresses.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}