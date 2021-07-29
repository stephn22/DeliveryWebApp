using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Application.Baskets.Queries
{
    public class GetBasketQuery : IRequest<Basket>
    {
        public int CustomerId { get; set; }
    }

    public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Basket>
    {
        private readonly IApplicationDbContext _context;

        public GetBasketQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Basket> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Baskets.Where(b => b.CustomerId == request.CustomerId)
                    .FirstAsync(cancellationToken);
            }
            catch (InvalidOperationException e)
            {
                return null;
            }
        }
    }
}
