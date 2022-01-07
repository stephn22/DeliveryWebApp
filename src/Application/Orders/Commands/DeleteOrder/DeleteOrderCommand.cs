using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.OrderItems.Commands.DeleteOrderItem;
using DeliveryWebApp.Application.OrderItems.Queries;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommand : IRequest<Order>
{
    public int Id { get; set; }
}

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Order>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public DeleteOrderCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Order> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Order), request.Id);
        }

        var orderItems = await _mediator.Send(new GetOrderItemsQuery
        {
            OrderId = entity.Id
        }, cancellationToken);

        // delete each orderItem
        foreach (var item in orderItems)
        {
            await _mediator.Send(new DeleteOrderItemCommand
            {
                Id = item.Id
            }, cancellationToken);
        }

        _context.Orders.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}