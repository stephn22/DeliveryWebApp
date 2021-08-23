using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Restaurateurs.Queries.GetSingleRestaurateur
{
    public class GetSingleRestaurateurQuery : IRequest<Restaurateur>
    {
        public int Id { get; set; }
    }

    public class GetSingleRestaurateurQueryHandler : IRequestHandler<GetSingleRestaurateurQuery, Restaurateur>
    {
        private readonly IApplicationDbContext _context;

        public GetSingleRestaurateurQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<Restaurateur> Handle(GetSingleRestaurateurQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Restaurateurs.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Restaurateur), request.Id);
            }

            return entity;
        }
    }
}
