using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.BasketItems.Commands.UpdateBasketItem
{
    public class UpdateBasketItemCommand : IRequest<BasketItem>
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateBasketItemCommandHandler : IRequestHandler<UpdateBasketItemCommand, BasketItem>
    {
        private readonly IApplicationDbContext _context;

        public UpdateBasketItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BasketItem> Handle(UpdateBasketItemCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.BasketItems.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(BasketItem), request.Id);
            }

            entity.ProductId = request.Product.Id;
            entity.Quantity = request.Quantity;

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
