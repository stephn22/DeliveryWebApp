using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommand : IRequest<Product>
    {
        public Product Product { get; set; }
    }

    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Product>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteProductCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Product> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Product>(request.Product);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Product.Id);
            }

            _context.Products.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
