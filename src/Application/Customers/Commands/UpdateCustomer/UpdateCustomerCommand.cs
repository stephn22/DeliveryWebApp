using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Customers.Commands.UpdateCustomer;

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

        if (!string.IsNullOrEmpty(request.Email))
        {
            entity.Email = request.Email;
        }

        if (!string.IsNullOrEmpty(request.FName))
        {
            entity.FirstName = request.FName;
        }

        if (!string.IsNullOrEmpty(request.LName))
        {
            entity.LastName = request.LName;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}