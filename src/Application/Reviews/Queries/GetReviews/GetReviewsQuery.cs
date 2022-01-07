using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Reviews.Queries.GetReviews;

public class GetReviewsQuery : IRequest<List<Review>>
{
    public int? CustomerId { get; set; }
    public int? RestaurateurId { get; set; }
}

public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, List<Review>>
{
    private readonly IApplicationDbContext _context;

    public GetReviewsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }


    public async Task<List<Review>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        // get reviews of a single customer
        if (request.CustomerId != null)
        {
            try
            {
                return await _context.Reviews.Where(r => r.CustomerId == request.CustomerId)
                    .ToListAsync(cancellationToken);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        // get reviews of a restaurateur
        if (request.RestaurateurId != null)
        {
            try
            {
                return await _context.Reviews.Where(r => r.RestaurateurId == request.RestaurateurId)
                    .ToListAsync(cancellationToken);
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        return null;
    }
}