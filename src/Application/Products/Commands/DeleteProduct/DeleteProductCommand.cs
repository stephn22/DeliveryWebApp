using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

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
            //var entity = await _context.Products.FindAsync(request.Id);

            //if (entity == null)
            //{
            //    throw new NotFoundException(nameof(Product), request.Id);
            //}

            //_context.Products.Remove(entity);

            var entity = _mapper.Map<Product>(request.Product);
            _context.Products.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
