using AutoMapper;
using AutoMapper.QueryableExtensions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Common.Mappings;
using DeliveryWebApp.Application.Common.Models;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Customers.Queries.GetCustomersWithPagination
{
    public class GetCustomersWithPaginationQuery : IRequest<PaginatedList<Customer>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class
        GetCustomersWithPaginationQueryHandler : IRequestHandler<GetCustomersWithPaginationQuery,
            PaginatedList<Customer>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCustomersWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<Customer>> Handle(GetCustomersWithPaginationQuery request,
            CancellationToken cancellationToken)
        {
            return await _context.Customers
                .OrderBy(c => c.Id)
                .ProjectTo<Customer>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}