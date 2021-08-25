using DeliveryWebApp.Application.Baskets.Commands.PurgeBasket;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.OrderItems.Commands.CreateOrderItem;
using DeliveryWebApp.Application.Orders.Extensions;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Order>
    {
        public Customer Customer { get; set; }
        public Restaurateur Restaurateur { get; set; }
        public IEnumerable<BasketItem> BasketItems { get; set; }
        public int AddressId { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public CreateOrderCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = new Order
            {
                RestaurateurId = request.Restaurateur.Id,
                CustomerId = request.Customer.Id,
                Status = OrderStatus.New,
                Date = DateTime.UtcNow,
                DeliveryAddressId = request.AddressId
            };

            _context.Orders.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            if (request.BasketItems.Any())
            {
                var basketId = 0;

                foreach (var item in request.BasketItems)
                {
                    basketId = item.BasketId;

                    await _mediator.Send(new CreateOrderItemCommand
                    {
                        BasketItemId = item.Id,
                        OrderId = entity.Id
                    }, cancellationToken);
                }

                // purge basket
                await _mediator.Send(new PurgeBasketCommand
                {
                    Id = basketId
                }, cancellationToken);
            }

            entity.TotalPrice = await entity.GetOrderTotalPrice(_mediator, _context);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
