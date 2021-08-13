using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Addresses.Queries.GetAddresses
{
    public class GetAddressesQuery : IRequest<List<Address>>
    {
        public int CustomerId { get; set; }
    }

    public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, List<Address>>
    {
        private readonly IApplicationDbContext _context;

        public GetAddressesQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Address>> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
        {
            // return list of addresses (max 2) of a customer
            try
            {
                return await (from a in _context.Addresses
                              where a.CustomerId == request.CustomerId
                              select a).ToListAsync(cancellationToken);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
