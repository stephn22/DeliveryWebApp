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
    public class UpdateCustomerCommand : IRequest<Customer>
    {
        public int Id { get; set; }
        public string Fname { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
    }

    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Customer>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Customers.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }

            if (string.IsNullOrEmpty(request.Email))
            {
                entity.Email = request.Email;
            }

            if (string.IsNullOrEmpty(request.Fname))
            {
                entity.FirstName = request.Fname;
            }

            if (string.IsNullOrEmpty(request.LName))
            {
                entity.LastName = request.LName;
            }

            // if Addresses is null instantiate a new list
            entity.Addresses ??= new List<Address>();
            entity.Addresses.Add(request.Address);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
