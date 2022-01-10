using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Restaurateurs.Queries.GetRestaurateurDetail;

public class GetRestaurateurDetailQuery : IRequest<Restaurateur>
{
    public int Id { get; set; }
}

public class GetSingleRestaurateurQueryHandler : IRequestHandler<GetRestaurateurDetailQuery, Restaurateur>
{
    private readonly IApplicationDbContext _context;

    public GetSingleRestaurateurQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<Restaurateur> Handle(GetRestaurateurDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Restaurateurs.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Restaurateur), request.Id);
        }

        return entity;
    }
}