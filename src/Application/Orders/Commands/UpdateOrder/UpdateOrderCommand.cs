using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.OrderItems.Commands.CreateOrderItem;
using DeliveryWebApp.Application.Orders.Extensions;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<Order>
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Order>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public UpdateOrderCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            if (request.Date != null)
            {
                entity.Date = (DateTime)request.Date;
            }

            if (request.DeliveryDate != null)
            {
                entity.DeliveryDate = request.DeliveryDate;
            }

            if (!string.IsNullOrEmpty(request.OrderStatus))
            {
                entity.Status = request.OrderStatus;
            }

            if (request.BasketItems is not { Count: > 0 })
            {
                return entity;
            }

            foreach (var item in request.BasketItems)
            {
                await _mediator.Send(new CreateOrderItemCommand
                {
                    BasketItemId = item.Id,
                    OrderId = entity.Id
                }, cancellationToken);
            }

            entity.TotalPrice = await entity.GetOrderTotalPrice(_mediator, _context);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
