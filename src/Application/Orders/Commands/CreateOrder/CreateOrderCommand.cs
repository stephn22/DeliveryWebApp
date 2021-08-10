using System;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.OrderItems.Commands.CreateOrderItem;
using DeliveryWebApp.Domain.Constants;

namespace DeliveryWebApp.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Order>
    {
        public Customer Customer { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
        public int RestaurateurId { get; set; }
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
                RestaurateurId = request.RestaurateurId,
                CustomerId = request.Customer.Id,
                Date = DateTime.Now,
                Status = OrderStatus.New,
            };

            // assign each BasketItem to an OrderItem
            foreach (var product in request.BasketItems)
            {
                await _mediator.Send(new CreateOrderItemCommand
                {
                    Order = entity
                }, cancellationToken);
            }

            // TODO: total price

            _context.Orders.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
