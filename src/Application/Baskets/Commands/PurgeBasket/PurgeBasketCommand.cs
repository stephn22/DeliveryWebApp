using AutoMapper;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Baskets.Commands.PurgeBasket
{
    [Authorize(Roles = RoleName.Admin)]
    [Authorize(Policy = PolicyName.IsCustomer)]
    public class PurgeBasketCommand : IRequest
    {
        public Basket Basket { get; set; }
    }

    public class PurgeBasketCommandHandler : IRequestHandler<PurgeBasketCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public PurgeBasketCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(PurgeBasketCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Basket>(request.Basket);


            entity.TotalPrice = 0.00M;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
