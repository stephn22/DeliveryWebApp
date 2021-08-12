using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.OrderItems.Commands.DeleteOrderItem
{
    public class DeleteOrderItemCommand : IRequest<OrderItem>
    {
        public int Id { get; set; }
    }

    public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, OrderItem>
    {
        private readonly IApplicationDbContext _context;

        public DeleteOrderItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderItem> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.OrderItems.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }

            _context.OrderItems.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
