using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Customers.Queries.GetCustomerDetail;

public class GetCustomerDetailQuery : IRequest<Customer>
{
    public int Id { get; set; }
}

public class GetSingleCustomerRequestQueryHandler : IRequestHandler<GetCustomerDetailQuery, Customer>
{
    private readonly IApplicationDbContext _context;

    public GetSingleCustomerRequestQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> Handle(GetCustomerDetailQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Customers.FindAsync(request.Id);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Customer), request.Id);
        }

        return entity;
    }
}