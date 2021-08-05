using System.Collections.Generic;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur
{
    public class UpdateRestaurateurCommand : IRequest<int>
    {
        public int Id { get; set; }
        public byte[] Logo { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantCategory { get; set; }
        public Address RestaurantAddress { get; set; }
        public Product Product { get; set; }
    }

    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurateurCommand, int>
    {
        private readonly IApplicationDbContext _context;

        public UpdateRestaurantCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateRestaurateurCommand request, CancellationToken cancellationToken)
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
                entity.RestaurantAddress = request.RestaurantAddress;
            }

            if (request.Product != null)
            {
                entity.Products ??= new List<Product>();
                entity.Products.Add(request.Product);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return request.Id;
        }
    }
}
