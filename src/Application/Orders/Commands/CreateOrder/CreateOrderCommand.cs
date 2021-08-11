using System;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.OrderItems.Commands.CreateOrderItem;
using DeliveryWebApp.Application.OrderItems.Queries;
using DeliveryWebApp.Domain.Constants;

namespace DeliveryWebApp.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Order>
    {
        public Customer Customer { get; set; }
        public Restaurateur Restaurateur { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IApplicationDbContext _context;

        public CreateOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = new Order
            {
                RestaurateurId = request.Restaurateur.Id,
                CustomerId = request.Customer.Id,
                Status = OrderStatus.New,
                TotalPrice = 0.00M
            };

            _context.Orders.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
