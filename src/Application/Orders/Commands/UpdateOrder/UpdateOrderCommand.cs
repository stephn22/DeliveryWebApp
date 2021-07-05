using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<Order>
    {
        public int Id { get; set; }
        public string OrderStatus { get; set; }
        public DateTime? DeliveryDate { get; set; }
    }

    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Order>
    {
        private readonly IApplicationDbContext _context;

        public UpdateOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Orders.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            if (request.DeliveryDate != null)
            {
                entity.DeliveryDate = request.DeliveryDate;
            }

            if (request.OrderStatus != null)
            {
                entity.Status = request.OrderStatus;
            }

            return entity;
        }
    }
}
