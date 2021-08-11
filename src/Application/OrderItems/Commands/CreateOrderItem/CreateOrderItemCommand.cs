using AutoMapper;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;

namespace DeliveryWebApp.Application.OrderItems.Commands.CreateOrderItem
{
    public class CreateOrderItemCommand : IRequest<OrderItem>
    {
        public Order Order { get; set; }
        public BasketItem BasketItem { get; set; }
    }

    public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand, OrderItem>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateOrderItemCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<OrderItem> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var order = _mapper.Map<Order>(request.Order);
            var basketItem = _mapper.Map<BasketItem>(request.BasketItem);

            if (order == null)
            {
                throw new NotFoundException(nameof(Order), request.Order.Id);
            }

            if (basketItem == null)
            {
                throw new NotFoundException(nameof(BasketItem), request.BasketItem.Id);
            }

            var product = await _context.Products.FindAsync(basketItem.ProductId);

            var entity = new OrderItem
            {
                ProductId = basketItem.ProductId,
                Discount = product.Discount,
                OrderId = request.Order.Id,
                ProductPrice = product.Price
            };

            return entity;
        }
    }
}
