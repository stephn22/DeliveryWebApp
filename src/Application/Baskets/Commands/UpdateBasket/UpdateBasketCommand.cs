using AutoMapper;
using DeliveryWebApp.Application.BasketItems.Commands.CreateBasketItem;
using DeliveryWebApp.Application.BasketItems.Queries;
using DeliveryWebApp.Application.Baskets.Extensions;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.BasketItems.Commands.UpdateBasketItem;

namespace DeliveryWebApp.Application.Baskets.Commands.UpdateBasket
{
    public class UpdateBasketCommand : IRequest<Basket>
    {
        public Basket Basket { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, Basket>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public UpdateBasketCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Basket> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
        {
            // TODO: check if mediator is useless

            var entity = _mapper.Map<Basket>(request.Basket);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Basket), request.Basket.Id);
            }

            var product = _mapper.Map<Product>(request.Product);

            if (product == null)
            {
                throw new NotFoundException(nameof(Product), request.Product.Id);
            }

            var basketItems = await _mediator.Send(new GetBasketItemsQuery
            {
                Basket = request.Basket
            }, cancellationToken);

            if (basketItems is { Count: > 0 })
            {
                try
                {
                    // search if there's an item with the same product id
                    var item = basketItems.First(b => b.ProductId == product.Id);

                    // update quantity
                    var newQty = item.Quantity + request.Quantity;

                    // update the existing basket item
                    await _mediator.Send(new UpdateBasketItemCommand
                    {
                        Id = item.Id,
                        Quantity = newQty,
                        Product = product
                    }, cancellationToken);
                }
                catch (InvalidOperationException) // if there's no element with that productId
                {
                }
            }
            else
            {   // create a new one
                await _mediator.Send(new CreateBasketItemCommand
                {
                    Basket = entity,
                    Product = product,
                    Quantity = request.Quantity
                }, cancellationToken);
            }

            entity.TotalPrice = await entity.GetBasketTotalPrice(_mediator, _context);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
