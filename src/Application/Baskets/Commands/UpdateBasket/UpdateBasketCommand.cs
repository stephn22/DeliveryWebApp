using AutoMapper;
using DeliveryWebApp.Application.BasketItems.Commands.CreateBasketItem;
using DeliveryWebApp.Application.BasketItems.Commands.UpdateBasketItem;
using DeliveryWebApp.Application.BasketItems.Queries;
using DeliveryWebApp.Application.Baskets.Commands.PurgeBasket;
using DeliveryWebApp.Application.Baskets.Extensions;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Baskets.Commands.UpdateBasket
{
    public class UpdateBasketCommand : IRequest<Basket>
    {
        public Basket Basket { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public int RestaurateurId { get; set; }
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
                // check if request.RestaurateurId is the same as the one in basketItems
                var p = await _context.Products.FindAsync(basketItems[0].ProductId);

                if (p != null)
                {
                    var r = await _context.Restaurateurs.FindAsync(p.RestaurateurId);

                    if (r.Id != request.RestaurateurId) // if not the same
                    {
                        // delete all current basket items (purge basket)
                        await _mediator.Send(new PurgeBasketCommand
                        {
                            Id = request.Basket.Id
                        }, cancellationToken);

                        // create a new one
                        await _mediator.Send(new CreateBasketItemCommand
                        {
                            Basket = entity,
                            Product = product,
                            Quantity = request.Quantity
                        }, cancellationToken);
                    }
                    else // if the same, continue
                    {
                        // search if there's an item with the same product id
                        var item = basketItems.Find(b => b.ProductId == product.Id);

                        if (item != null)
                        {
                            // update quantity
                            var newQty = item.Quantity + request.Quantity;

                            // update the existing basket item
                            await _mediator.Send(new UpdateBasketItemCommand
                            {
                                Id = item.Id,
                                Quantity = newQty,
                            }, cancellationToken);
                        }
                        else // otherwise create a new one
                        {
                            await _mediator.Send(new CreateBasketItemCommand
                            {
                                Basket = entity,
                                Product = product,
                                Quantity = request.Quantity
                            }, cancellationToken);
                        }
                    }
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
