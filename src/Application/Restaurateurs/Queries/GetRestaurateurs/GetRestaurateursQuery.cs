using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Restaurateurs.Queries.GetRestaurateurs;

public class GetRestaurateursQuery : IRequest<List<Restaurateur>>
{
}

public class GetRestaurateursQueryHandler : IRequestHandler<GetRestaurateursQuery, List<Restaurateur>>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<GetRestaurateursQuery> _logger;

    public GetRestaurateursQueryHandler(IApplicationDbContext context, ILogger<GetRestaurateursQuery> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Restaurateur>> Handle(GetRestaurateursQuery request, CancellationToken cancellationToken)
    {
        try
        {
            return await _context.Restaurateurs.ToListAsync(cancellationToken);
        }
        catch (InvalidOperationException e)
        {
            _logger.LogWarning($"{nameof(Restaurateur)}, {e.Message}");
            return null;
        }
    }
}