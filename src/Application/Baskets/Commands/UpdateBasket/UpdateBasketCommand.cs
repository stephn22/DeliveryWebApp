using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Domain.Objects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Application.Baskets.Commands.UpdateBasket
{
    public class UpdateBasketCommand : IRequest<Basket>
    {
        public AddToBasket AddToBasket { get; set; }
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
            // FIXME: request.AddToBasket is null
            var entity = await _context.Baskets.Where(b => b.CustomerId == request.AddToBasket.CustomerId)
                .FirstAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Basket), request.AddToBasket.CustomerId);
            }

            // if Products is null instantiate a new list
            entity.Products ??= new List<Product>();
            entity.Products.Add(request.AddToBasket.Product);

            entity.TotalPrice = Product.TotalPrice(entity.Products.ToList());

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
