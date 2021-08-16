using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;

namespace DeliveryWebApp.Application.OrderItems.Commands.UpdateOrderItem
{
    public class UpdateOrderItemCommand : IRequest<OrderItem>
    {
        public int Id { get; set; }
        public decimal? ProductPrice { get; set; }
        public int? Discount { get; set; }
        public int? Quantity { get; set; }
    }

    public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand, OrderItem>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public UpdateOrderItemCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<OrderItem> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.OrderItems.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(OrderItem), request.Id);
            }

            if (request.ProductPrice != null)
            {
                entity.ProductPrice = (decimal)request.ProductPrice;
            }

            if (request.Discount != null)
            {
                entity.Discount = (int)request.Discount;
            }

            if (request.Quantity != null)
            {
                entity.Quantity = (int)request.Quantity;

                var product = await _context.Products.FindAsync(entity.ProductId);

                if (product == null)
                {
                    throw new NotFoundException(nameof(Product), entity.ProductId);
                }

                // update product quantity
                var newQuantity = product.Quantity;

                if (request.Quantity < entity.Quantity)
                {
                    newQuantity += (entity.Quantity - (int)request.Quantity);
                }
                else
                {
                    newQuantity -= ((int)request.Quantity - entity.Quantity);
                }

                await _mediator.Send(new UpdateProductCommand
                {
                    Id = product.Id,
                    Quantity = newQuantity
                }, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
