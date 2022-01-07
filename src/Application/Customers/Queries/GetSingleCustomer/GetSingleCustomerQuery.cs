using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Customers.Queries.GetSingleCustomer;

public class GetSingleCustomerQuery : IRequest<Customer>
{
    public int Id { get; set; }
}

public class GetSingleCustomerRequestQueryHandler : IRequestHandler<GetSingleCustomerQuery, Customer>
{
    private readonly IApplicationDbContext _context;

    public GetSingleCustomerRequestQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> Handle(GetSingleCustomerQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Customer), request.Id);
        }

        return entity;
    }
}