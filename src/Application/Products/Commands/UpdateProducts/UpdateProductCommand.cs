using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Products.Commands.UpdateProducts;

public class UpdateProductCommand : IRequest<Product>
{
    public int Id { get; set; }
    public int? Discount { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public byte[] Image { get; set; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Product>
{
    private readonly IApplicationDbContext _context;

    public UpdateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        if (request.Discount != null)
        {
            entity.Discount = (int)request.Discount;
        }

        if (request.Quantity != null)
        {
            entity.Quantity = (int)request.Quantity;
        }

        if (request.Price != null)
        {
            entity.Price = (decimal)request.Price;
        }

        if (!string.IsNullOrEmpty(request.Category))
        {
            entity.Category = request.Category;
        }

        if (!string.IsNullOrEmpty(request.Name))
        {
            entity.Name = request.Name;
        }

        if (request.Image != null)
        {
            entity.Image = request.Image;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}