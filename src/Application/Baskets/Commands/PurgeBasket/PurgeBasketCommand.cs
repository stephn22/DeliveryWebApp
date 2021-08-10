using AutoMapper;
using DeliveryWebApp.Application.BasketItems.Commands.DeleteBasketItem;
using DeliveryWebApp.Application.BasketItems.Queries;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Baskets.Commands.PurgeBasket
{
    //[Authorize(Roles = RoleName.Admin)]
    //[Authorize(Policy = PolicyName.IsCustomer)] TODO:
    public class PurgeBasketCommand : IRequest
    {
        public Basket Basket { get; set; }
    }

    public class PurgeBasketCommandHandler : IRequestHandler<PurgeBasketCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public PurgeBasketCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(PurgeBasketCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Basket>(request.Basket);

            var basketItems = await _mediator.Send(new GetBasketItemsQuery
            {
                Basket = entity
            }, cancellationToken);

            foreach (var item in basketItems)
            {
                try
                {
                    await _mediator.Send(new DeleteBasketItemCommand
                    {
                        BasketItem = item
                    }, cancellationToken);
                }
                catch (NotFoundException)
                { }
            }

            entity.TotalPrice = 0.00M;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
