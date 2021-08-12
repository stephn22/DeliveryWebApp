using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.BasketItems.Commands.DeleteBasketItem
{
    public class DeleteBasketItemCommand : IRequest<BasketItem>
    {
        public int Id { get; set; }
    }

    public class DeleteBasketItemCommandHandler : IRequestHandler<DeleteBasketItemCommand, BasketItem>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public DeleteBasketItemCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<BasketItem> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.BasketItems.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(BasketItem), request.Id);
            }

            var product = await _context.Products.FindAsync(entity.ProductId);

            var newQuantity = product.Quantity + entity.Quantity;

            await _mediator.Send(new UpdateProductCommand
            {
                Id = product.Id,
                Quantity = newQuantity
            }, cancellationToken);

            _context.BasketItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }

}
