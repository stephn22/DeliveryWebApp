using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.TodoLists.Queries.GetTodos;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Application.Clients.Queries.GetClients
{
    public class GetClientsQuery : IRequest<List<Client>>
    {
    }

    public class GetClientsQueryHandler : IRequestHandler<GetClientsQuery, List<Client>>
    {
        private readonly IApplicationDbContext _context;

        public GetClientsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Client>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
        {
            return await _context.Clients.ToListAsync(cancellationToken);
        }
    }
}
