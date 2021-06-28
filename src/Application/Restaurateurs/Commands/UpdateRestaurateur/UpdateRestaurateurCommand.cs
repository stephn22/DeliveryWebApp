using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Requests.Commands.UpdateRequest;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur
{
    public class UpdateRestaurateurCommand : IRequest
    {
        public int Id { get; set; }
        public Restaurant Restaurant { get; set; }
    }

    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurateurCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateRestaurantCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateRestaurateurCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Restaurateurs.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Restaurateur), request.Id);
            }

            entity.Restaurant = request.Restaurant;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
