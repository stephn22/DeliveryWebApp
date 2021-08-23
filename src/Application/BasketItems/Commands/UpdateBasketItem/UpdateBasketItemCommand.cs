using DeliveryWebApp.Application.Baskets.Extensions;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Baskets.Extensions;

namespace DeliveryWebApp.Application.BasketItems.Commands.UpdateBasketItem
{
    public class UpdateBasketItemCommand : IRequest<BasketItem>
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateBasketItemCommandHandler : IRequestHandler<UpdateBasketItemCommand, BasketItem>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public UpdateBasketItemCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<BasketItem> Handle(UpdateBasketItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.BasketItems.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(BasketItem), request.Id);
            }

            entity.Quantity = request.Quantity;

            var basket = await _context.Baskets.FindAsync(entity.BasketId);

            if (basket != null)
            {
                basket.TotalPrice = await basket.GetBasketTotalPrice(_mediator, _context);
                _context.Baskets.Update(basket);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
