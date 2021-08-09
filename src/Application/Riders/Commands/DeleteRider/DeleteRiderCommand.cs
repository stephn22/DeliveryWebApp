using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Riders.Commands.DeleteRider
{
    public class DeleteRiderCommand : IRequest
    {
        public Rider Rider { get; set; }
    }

    public class DeleteRiderCommandHandler : IRequestHandler<DeleteRiderCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteRiderCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteRiderCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Rider>(request.Rider);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Restaurateur), request.Rider);
            }

            _context.Riders.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
