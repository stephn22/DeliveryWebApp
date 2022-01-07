using DeliveryWebApp.Application.Baskets.Commands.PurgeBasket;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Baskets.Commands.DeleteBasket;

public class DeleteBasketCommand : IRequest<Basket>
{
    public int Id { get; set; }
}

public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, Basket>
{
    private readonly IApplicationDbContext _context;
    private readonly IMediator _mediator;

    public DeleteBasketCommandHandler(IApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<Basket> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Baskets.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Basket), request.Id);
        }

        await _mediator.Send(new PurgeBasketCommand
        {
            Id = entity.Id
        }, cancellationToken);

        _context.Baskets.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}