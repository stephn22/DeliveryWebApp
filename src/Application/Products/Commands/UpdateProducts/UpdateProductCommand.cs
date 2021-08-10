using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Products.Commands.UpdateProducts
{
    public class UpdateProductCommand : IRequest<Product>
    {
        public int Id { get; set; }
        public int Discount { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public byte[] Image { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateProductCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            entity.Discount = request.Discount;
            entity.Quantity = request.Quantity;
            entity.Price = request.Price;
            entity.Category = request.Category;
            entity.Name = request.Name;
            entity.Image = request.Image;

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
