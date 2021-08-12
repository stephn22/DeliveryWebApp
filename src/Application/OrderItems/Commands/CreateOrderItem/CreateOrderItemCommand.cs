using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.OrderItems.Commands.CreateOrderItem
{
    public class CreateOrderItemCommand : IRequest<OrderItem>
    {
        public int OrderId { get; set; }
        public int BasketItemId { get; set; }
    }

    public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand, OrderItem>
    {
        private readonly IApplicationDbContext _context;

        public CreateOrderItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderItem> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var order = await _context.Orders.FindAsync(request.OrderId);
            var basketItem = await _context.BasketItems.FindAsync(request.BasketItemId);

            if (order == null)
            {
                throw new NotFoundException(nameof(Order), request.OrderId);
            }

            if (basketItem == null)
            {
                throw new NotFoundException(nameof(BasketItem), request.BasketItemId);
            }

            var product = await _context.Products.FindAsync(basketItem.ProductId);

            var entity = new OrderItem
            {
                ProductId = basketItem.ProductId,
                Discount = product.Discount,
                OrderId = order.Id,
                ProductPrice = product.Price,
                Quantity = basketItem.Quantity
            };

            _context.OrderItems.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
