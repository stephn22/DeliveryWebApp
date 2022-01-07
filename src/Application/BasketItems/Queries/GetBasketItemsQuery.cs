using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.BasketItems.Queries;

public class GetBasketItemsQuery : IRequest<List<BasketItem>>
{
    public Basket Basket { get; set; }
}

public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQuery, List<BasketItem>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetBasketItemsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BasketItem>> Handle(GetBasketItemsQuery request, CancellationToken cancellationToken)
    {
        var basket = _mapper.Map<Basket>(request.Basket);

        if (basket == null)
        {
            throw new NotFoundException(nameof(Basket), request.Basket.Id);
        }

        try
        {
            var basketItemsList = await _context.BasketItems.Where(b => b.BasketId == basket.Id)
                .ToListAsync(cancellationToken);

            return basketItemsList;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
}