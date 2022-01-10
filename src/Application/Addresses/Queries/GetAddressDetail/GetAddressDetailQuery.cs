using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Addresses.Queries.GetAddressDetail;

/// <summary>
/// Used in controllers
/// </summary>
public class GetAddressDetailQuery : IRequest<Address>
{
    /// <summary>
    /// Address Id
    /// </summary>
    public int Id { get; set; }
}

public class GetSingleAddressQueryHandler : IRequestHandler<GetAddressDetailQuery, Address>
{
    private readonly IApplicationDbContext _context;

    public GetSingleAddressQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Address> Handle(GetAddressDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Addresses.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Address), request.Id);
        }

        return entity;
    }
}