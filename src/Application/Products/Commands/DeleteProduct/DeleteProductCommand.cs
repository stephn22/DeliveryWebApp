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
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public byte[] Image { get; set; }
        //public decimal Price { get; set; }
        //public int Discount { get; set; }
        //public string Category { get; set; }
        //public int Quantity { get; set; }
        //public int RestaurateurId { get; set; } // TODO: check
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

            var entity = _mapper.Map<Product>(request);
            _context.Products.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
