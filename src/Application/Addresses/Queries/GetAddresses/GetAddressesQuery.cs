using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            // return list of addresses (2) of a customer
            try
            {
                return await (from c in _context.Customers
                    where c.Id == request.CustomerId
                    select c.Addresses.ToList()).FirstAsync(cancellationToken);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
    }
}
