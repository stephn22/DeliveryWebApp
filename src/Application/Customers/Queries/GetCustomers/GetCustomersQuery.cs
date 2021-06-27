using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Application.Customers.Queries.GetCustomers
{
    public class GetCustomersQuery : IRequest<List<Customer>>
    {
    }

    public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<Customer>>
    {
        private readonly IApplicationDbContext _context;

        public GetCustomersQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
        {
            return await _context.Customers.ToListAsync(cancellationToken);
        }
    }
}
