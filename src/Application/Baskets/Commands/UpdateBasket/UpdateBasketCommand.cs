using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Baskets.Commands.UpdateBasket
{
    public class UpdateBasketCommand : IRequest
    {
        public int BasketId { get; set; }
        public Product Product { get; set; }
    }

    public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateBasketCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Baskets.FindAsync(request.BasketId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Basket), request.BasketId);
            }

            var products = new Collection<Product> {request.Product};

            entity.Products = products;
            entity.TotalPrice += request.Product.Price;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
