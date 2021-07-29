using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Baskets.Commands.UpdateBasket
{
    public class UpdateBasketCommand : IRequest<Basket>
    {
        public int CustomerId { get; set; }
        public Product Product { get; set; }
    }

    public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketCommand, Basket>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateBasketCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Basket> Handle(UpdateBasketCommand request, CancellationToken cancellationToken)
        {
            // FIXME: if basket not exist create a new one
            var entity = await _context.Baskets.Where(b => b.CustomerId == request.CustomerId)
                .FirstAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Basket), request.CustomerId);
            }

            // if Products is null instantiate a new list
            entity.Products ??= new List<Product>();
            entity.Products.Add(request.Product);

            entity.TotalPrice = Product.TotalPrice(entity.Products.ToList());

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
