using DeliveryWebApp.Application.BasketItems.Commands.DeleteBasketItem;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Product>
    {
        public int Id { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Product>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public DeleteProductCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Product> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            // remove the product from all basket items
            var basketItems = await _context.BasketItems.Where(b => b.ProductId == entity.Id).ToListAsync(cancellationToken);

            if (basketItems != null)
            {
                foreach (var item in basketItems)
                {
                    await _mediator.Send(new DeleteBasketItemCommand
                    {
                        Id = item.Id
                    }, cancellationToken);
                }
            }

            _context.Products.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
