using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Products.Commands.DeleteProduct;
using DeliveryWebApp.Application.Products.Queries.GetProducts;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Restaurateurs.Commands.DeleteRestaurateur
{
    public class DeleteRestaurateurCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteRestaurateurCommandHandler : IRequestHandler<DeleteRestaurateurCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMediator _mediator;

        public DeleteRestaurateurCommandHandler(IApplicationDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(DeleteRestaurateurCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Restaurateurs.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Restaurateur), request.Id);
            }

            // search if restaurateur has products

            var products = await _mediator.Send(new GetProductsQuery
            {
                RestaurateurId = entity.Id
            }, cancellationToken);

            if (products is { Count: > 0 })
            {
                foreach (var product in products)
                {
                    await _mediator.Send(new DeleteProductCommand
                    {
                        Id = product.Id
                    }, cancellationToken);
                }
            }

            _context.Restaurateurs.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
