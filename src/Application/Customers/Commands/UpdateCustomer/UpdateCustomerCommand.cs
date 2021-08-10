using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommand : IRequest<Customer>
    {
        public int Id { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }
        public string Email { get; set; }
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

            entity.Email = request.Email;
            entity.FirstName = request.FName;
            entity.LastName = request.LName;

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
