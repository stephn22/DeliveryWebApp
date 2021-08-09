using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Restaurateurs.Commands.DeleteRestaurateur
{
    public class DeleteRestaurateurCommand : IRequest
    {
        public Restaurateur Restaurateur { get; set; }
    }

    public class DeleteRestaurateurCommandHandler : IRequestHandler<DeleteRestaurateurCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteRestaurateurCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteRestaurateurCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Restaurateur>(request.Restaurateur);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Restaurateur), request.Restaurateur);
            }

            _context.Restaurateurs.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
