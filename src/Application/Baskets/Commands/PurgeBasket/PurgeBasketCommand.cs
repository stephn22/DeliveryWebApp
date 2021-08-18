using AutoMapper;
using DeliveryWebApp.Application.BasketItems.Commands.DeleteBasketItem;
using DeliveryWebApp.Application.BasketItems.Queries;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Baskets.Commands.PurgeBasket
{
    public class PurgeBasketCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class PurgeBasketCommandHandler : IRequestHandler<PurgeBasketCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public PurgeBasketCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(PurgeBasketCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Baskets.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Basket), request.Id);
            }

            var basketItems = await _mediator.Send(new GetBasketItemsQuery
            {
                Basket = entity
            }, cancellationToken);

            foreach (var item in basketItems)
            {
                try
                {
                    await _mediator.Send(new DeleteBasketItemCommand
                    {
                        Id = item.Id
                    }, cancellationToken);
                }
                catch (NotFoundException)
                { }
            }

            entity.TotalPrice = 0;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
