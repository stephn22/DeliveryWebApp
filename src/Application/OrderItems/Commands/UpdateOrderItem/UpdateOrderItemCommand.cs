using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;

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

        public UpdateOrderItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
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
            }

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
