using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Baskets.Commands.CreateBasket
{
    public class CreateBasketCommand : IRequest<int>
    {
        public Customer Customer { get; set; }
    }

    public class CreateBasketCommandHandler : IRequestHandler<CreateBasketCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateBasketCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
        {
            var entity = new Basket
            {
                Customer = request.Customer,
                Products = new List<Product>()
            };

            _context.Baskets.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
