using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.BasketItems.Commands.UpdateBasketItem
{
    public class UpdateBasketItemCommand : IRequest<BasketItem>
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int? Quantity { get; set; }
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

            if (request.Product != null)
            {
                entity.ProductId = request.Product.Id;
                entity.ProductPrice = request.Product.Price;
                entity.Discount = request.Product.Discount;
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
