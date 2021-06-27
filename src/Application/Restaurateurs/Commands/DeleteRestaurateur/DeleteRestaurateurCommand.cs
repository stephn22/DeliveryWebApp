using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
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

        public DeleteRestaurateurCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteRestaurateurCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Restaurateurs.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Restaurateur), request.Id);
            }

            _context.Restaurateurs.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
