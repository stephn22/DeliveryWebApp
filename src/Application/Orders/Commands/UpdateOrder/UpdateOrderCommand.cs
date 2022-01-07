using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommand : IRequest<Order>
{
    public int Id { get; set; }
    public string OrderStatus { get; set; }
    public DateTime? DeliveryDate { get; set; }
    public Rider Rider { get; set; }
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Order>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public UpdateOrderCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }

        if (request.DeliveryDate != null)
        {
            entity.DeliveryDate = request.DeliveryDate;
        }

        if (!string.IsNullOrEmpty(request.OrderStatus))
        {
            entity.Status = request.OrderStatus;
        }

        if (request.Rider != null)
        {
            entity.RiderId = request.Rider.Id;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}