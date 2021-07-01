using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Products.Commands.UpdateProducts
{
    public class UpdateProductCommand : IRequest
    {
        public int Id { get; set; }
        public int? Discount { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
    }

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateProductCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            if (request.Discount != null)
            {
                entity.Discount = (int) request.Discount;
            }

            if (request.Quantity != null)
            {
                entity.Quantity = (int) request.Quantity;
            }

            if (request.Price != null)
            {
                entity.Price = (double) request.Price;
            }

            if (request.Category != null)
            {
                entity.Category = request.Category;
            }

            if (request.Name != null)
            {
                entity.Name = request.Name;
            }

            if (request.ImageUrl != null)
            {
                entity.ImageUrl = request.ImageUrl;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
