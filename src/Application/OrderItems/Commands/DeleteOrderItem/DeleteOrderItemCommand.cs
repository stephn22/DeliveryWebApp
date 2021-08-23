using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;

namespace DeliveryWebApp.Application.OrderItems.Commands.DeleteOrderItem
{
    public class DeleteOrderItemCommand : IRequest<OrderItem>
    {
        public int Id { get; set; }
    }

    public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, OrderItem>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public DeleteOrderItemCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<OrderItem> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.OrderItems.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            var product = await _context.Products.FindAsync(entity.ProductId);

            if (product == null)
            {
                throw new NotFoundException(nameof(Product), entity.ProductId);
            }

            // update product quantity
            var newQuantity = product.Quantity + entity.Quantity;

            await _mediator.Send(new UpdateProductCommand
            {
                Id = product.Id,
                Quantity = newQuantity
            }, cancellationToken);

            _context.OrderItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
