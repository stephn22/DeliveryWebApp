using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Products.Commands.CreateProduct
{
    public class CreateProductCommand : IRequest<Product>
    {
        public string Name { get; set; }
        public byte[] Image { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public Restaurateur Restaurateur { get; set; }
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private readonly IApplicationDbContext _context;

        public CreateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = new Product
            {
                Name = request.Name,
                Image = request.Image,
                Price = request.Price,
                Discount = request.Discount,
                Category = request.Category,
                Quantity = request.Quantity,
                RestaurateurId = request.Restaurateur.Id
            };

            _context.Products.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
