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

namespace DeliveryWebApp.Application.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommand : IRequest<Address>
    {
        /// <summary>
        /// Address id
        /// </summary>
        public int Id { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Address>
    {
        private readonly IApplicationDbContext _context;

        public UpdateAddressCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Addresses.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Address), request.Id);
            }

            if (request.AddressLine1 != null)
            {
                entity.AddressLine1 = request.AddressLine1;
            }

            if (request.AddressLine2 != null)
            {
                entity.AddressLine2 = request.AddressLine2;
            }

            if (request.Number != null)
            {
                entity.Number = request.Number;
            }

            if (request.City != null)
            {
                entity.City = request.City;
            }

            if (request.PostalCode != null)
            {
                entity.PostalCode = request.PostalCode;
            }

            if (request.Country != null)
            {
                entity.Country = request.Country;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
