using AutoMapper;
using DeliveryWebApp.Application.BasketItems.Commands.CreateBasketItem;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Baskets.Extensions;

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

            var basketItem = await _mediator.Send(new CreateBasketItemCommand
            {
                Basket = entity,
                Product = product,
                Quantity = request.Quantity
            }, cancellationToken);

            entity.TotalPrice = await entity.GetBasketTotalPrice(_mediator, _context);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
