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

namespace DeliveryWebApp.Application.Addresses.Queries.GetSingleAddress
{
    public class GetSingleAddressQuery : IRequest<Address>
    {
        public int Id { get; set; }
    }

    public class GetSingleAddressQueryHandler : IRequestHandler<GetSingleAddressQuery, Address>
    {
        private readonly IApplicationDbContext _context;

        public GetSingleAddressQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address> Handle(GetSingleAddressQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Addresses.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Address), request.Id);
            }

            return entity;
        }
    }
}
