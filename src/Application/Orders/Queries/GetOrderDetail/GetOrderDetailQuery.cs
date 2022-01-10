using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Orders.Queries.GetOrderDetail;

public class GetOrderDetailQuery : IRequest<Order>
{
    public int Id { get; set; }
}

public class GetSingleOrderQueryHandler : IRequestHandler<GetOrderDetailQuery, Order>
{
    private readonly IApplicationDbContext _context;

    public GetSingleOrderQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Order> Handle(GetOrderDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }

        return entity;
    }
}