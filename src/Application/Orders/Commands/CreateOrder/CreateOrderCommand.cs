using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<int>
    {
        public Customer Customer { get; set; }
        public ICollection<Product> Products { get; set; }
        public Restaurant Restaurant { get; set; }
    }

    public class CreateOrderCommndHandler : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public CreateOrderCommndHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var totalPrice = request.Products.Sum(product => product.Price);

            var entity = new Order
            {
                Customer = request.Customer,
                Date = DateTime.Now,
                Products = request.Products,
                TotalPrice = totalPrice,
                Restaurant = request.Restaurant,
                Status = OrderStatus.Open
            };

            _context.Orders.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
