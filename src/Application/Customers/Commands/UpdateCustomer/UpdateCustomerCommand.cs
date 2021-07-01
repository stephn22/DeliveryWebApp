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

namespace DeliveryWebApp.Application.Customers.Commands.UpdateCustomer
{
    /// <summary>
    /// Called when customer add address(es) for the first time
    /// </summary>
    public class UpdateCustomerCommand : IRequest
    {
        public int Id { get; set; }
        public Address Address { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Customers.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }
            // instantiate new 
            entity.Addresses = new List<Address>();
            entity.Addresses.Add(request.Address);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
