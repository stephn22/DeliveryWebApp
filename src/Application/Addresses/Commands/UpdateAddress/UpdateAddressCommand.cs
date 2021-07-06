using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Addresses.Commands.UpdateAddress
{
    public class UpdateAddressCommand : IRequest<Address>
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

    public class UpdateAddressCommandHandler : IRequestHandler<UpdateAddressCommand, Address>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateAddressCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Address> Handle(UpdateAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Addresses.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Address), request.Id);
            }

            entity.AddressLine1 = request.AddressLine1;
            entity.AddressLine2 = request.AddressLine2;
            entity.Number = request.Number;
            entity.City = request.City;
            entity.PostalCode = entity.PostalCode;
            entity.StateProvince = entity.StateProvince;
            entity.Country = entity.Country;

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
