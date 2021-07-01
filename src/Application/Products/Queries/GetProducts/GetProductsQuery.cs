using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<List<Product>>
    {
        public int? RestaurantId { get; set; }
        public int? OrderId { get; set; }
    }

    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<Product>>
    {
        private readonly IApplicationDbContext _context;

        public GetProductsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            if (request.RestaurantId != null)
            {
                return await (from r in _context.Restaurants
                    where r.Id == request.RestaurantId
                    select r.Products.ToList()).FirstAsync(cancellationToken); // TODO: check
            }

            if (request.OrderId != null)
            {
                return await (from o in _context.Orders
                    where o.Id == request.OrderId
                    select o.Products.ToList()).FirstAsync(cancellationToken); // TODO: check
            }

            throw new NotFoundException();
        }
    }
}
