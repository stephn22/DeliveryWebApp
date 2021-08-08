using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.Application.BasketItems.Commands.CreateBasketItem
{
    public class CreateBasketItemCommand : IRequest<BasketItem>
    {
        public int BasketId { get; set; }
        public Product Product { get; set; }
    }

    public class CreateBasketItemCommandHandler : IRequestHandler<CreateBasketItemCommand, BasketItem>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<CreateBasketItemCommand> _logger;

        public CreateBasketItemCommandHandler(IApplicationDbContext context, ILogger<CreateBasketItemCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<BasketItem> Handle(CreateBasketItemCommand request, CancellationToken cancellationToken)
        {
            var entity = new BasketItem
            {
                BasketId = request.BasketId,
                Product = request.Product
            };

            _context.BasketItems.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
