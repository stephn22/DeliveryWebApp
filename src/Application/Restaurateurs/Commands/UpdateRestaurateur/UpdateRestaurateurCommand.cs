using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

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
