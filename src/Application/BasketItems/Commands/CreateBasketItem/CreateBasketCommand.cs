using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.BasketItems.Commands.CreateBasketItem
{
    public class CreateBasketItemCommand : IRequest<BasketItem>
    {
        public Basket Basket { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateBasketItemCommandHandler : IRequestHandler<CreateBasketItemCommand, BasketItem>
    {
        private readonly IApplicationDbContext _context;

        public CreateBasketItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BasketItem> Handle(CreateBasketItemCommand request, CancellationToken cancellationToken)
        {
            var entity = new BasketItem
            {
                BasketId = request.Basket.Id,
                ProductId = request.Product.Id,
                Discount = request.Product.Discount,
                ProductPrice = request.Product.Price,
                Quantity = request.Quantity
            };

            _context.BasketItems.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
