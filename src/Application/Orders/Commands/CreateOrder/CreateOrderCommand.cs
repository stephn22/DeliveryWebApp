using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Order>
    {
        public Customer Customer { get; set; }
        public ICollection<Product> Products { get; set; }
        public int RestaurateurId { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
    {
        private readonly IApplicationDbContext _context;

        public CreateOrderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var totalPrice = Product.TotalPrice(request.Products.ToList());

            var entity = new Order
            {
                // TODO: complete
            };

            _context.Orders.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
