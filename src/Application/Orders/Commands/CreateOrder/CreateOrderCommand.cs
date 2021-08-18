﻿using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.OrderItems.Commands.CreateOrderItem;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Orders.Extensions;

namespace DeliveryWebApp.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Order>
    {
        public Customer Customer { get; set; }
        public Restaurateur Restaurateur { get; set; }
        public IEnumerable<BasketItem> BasketItems { get; set; }
        public Address Address { get; set; }
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
                AddressId = request.Address.Id
            };

            _context.Orders.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            if (request.BasketItems.Any())
            {
                foreach (var item in request.BasketItems)
                {
                    await _mediator.Send(new CreateOrderItemCommand
                    {
                        BasketItemId = item.Id,
                        OrderId = entity.Id
                    }, cancellationToken);
                }
            }

            entity.TotalPrice = await entity.GetOrderTotalPrice(_mediator, _context);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
