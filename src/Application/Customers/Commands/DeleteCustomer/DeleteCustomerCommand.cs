using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<Customer>
    {
        public int Id { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Customer>
    {
        private readonly IApplicationDbContext _context;

        public DeleteCustomerCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Customers.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Customer), request.Id);
            }

            var addresses = await _context.Addresses.Where(a => a.CustomerId == entity.Id)
                .ToListAsync(cancellationToken);

            _context.Addresses.RemoveRange(addresses);
            _context.Customers.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}