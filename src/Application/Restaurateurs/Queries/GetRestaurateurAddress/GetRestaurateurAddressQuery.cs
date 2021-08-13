using System;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.Application.Restaurateurs.Queries.GetRestaurateurAddress
{
    public class GetRestaurateurAddressQuery : IRequest<Address>
    {
        /// <summary>
        /// Address Id
        /// </summary>
        public int Id { get; set; }
    }

    public class GetRestaurateurAddressQueryHandler : IRequestHandler<GetRestaurateurAddressQuery, Address>
    {
        private readonly IApplicationDbContext _context;

        public GetRestaurateurAddressQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address> Handle(GetRestaurateurAddressQuery request, CancellationToken cancellationToken)
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