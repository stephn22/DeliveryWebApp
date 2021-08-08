using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Baskets.Commands.CreateBasket
{
    public class CreateBasketCommand : IRequest<Basket>
    {
        public Customer Customer { get; set; }
    }

    public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, Basket>
    {
        private readonly IApplicationDbContext _context;

        public CreateBasketCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Basket> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            var entity = new Basket
            {
                CustomerId = request.Customer.Id
            };

            _context.Baskets.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
