using AutoMapper;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Baskets.Queries
{
    public class GetBasketQuery : IRequest<Basket>
    {
        public Customer Customer { get; set; }
    }

    public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Basket>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetBasketQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Basket> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var customer = _mapper.Map<Customer>(request.Customer);

            try
            {
                var entity = await _context.Baskets.FirstAsync(b => b.CustomerId == customer.Id, cancellationToken);
                return entity;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
