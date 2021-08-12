using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Riders.Commands.DeleteRider
{
    public class DeleteRiderCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteRiderCommandHandler : IRequestHandler<DeleteRiderCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeleteRiderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteRiderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Riders.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Restaurateur), request.Id);
            }

            _context.Riders.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
