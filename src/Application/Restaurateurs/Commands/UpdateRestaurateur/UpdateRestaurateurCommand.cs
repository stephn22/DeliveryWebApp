using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur
{
    public class UpdateRestaurateurCommand : IRequest<Restaurateur>
    {
        public int Id { get; set; }
        public byte[] Logo { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantCategory { get; set; }
        public Address RestaurantAddress { get; set; }
    }

    public class UpdateRestaurateurCommandHandler : IRequestHandler<UpdateRestaurateurCommand, Restaurateur>
    {
        private readonly IApplicationDbContext _context;

        public UpdateRestaurateurCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Restaurateur> Handle(UpdateRestaurateurCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Restaurateurs.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Restaurateur), request.Id);
            }

            if (request.Logo != null)
            {
                entity.Logo = request.Logo;
            }

            if (!string.IsNullOrEmpty(request.RestaurantName))
            {
                entity.RestaurantName = request.RestaurantName;
            }

            if (!string.IsNullOrEmpty(request.RestaurantCategory))
            {
                entity.RestaurantCategory = request.RestaurantCategory;
            }

            if (request.RestaurantAddress != null)
            {
                entity.RestaurantAddressId = request.RestaurantAddress.Id;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
