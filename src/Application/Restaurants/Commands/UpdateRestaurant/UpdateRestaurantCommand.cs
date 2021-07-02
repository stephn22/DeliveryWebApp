using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommand : IRequest
    {
        public int Id { get; set; }
        public byte[] Logo { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public Address Address { get; set; }
        public Product Product { get; set; }
    }

    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand>
    {
        private readonly IApplicationDbContext _context;

        public UpdateRestaurantCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Restaurants.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Restaurant), request.Id);
            }

            if (request.Logo != null)
            {
                entity.Logo = request.Logo;
            }

            if (request.Name != null)
            {
                entity.Name = request.Name;
            }

            if (request.Category != null)
            {
                entity.Category = request.Category;
            }

            if (request.Address != null)
            {
                entity.Address = request.Address;
            }

            if (request.Product != null)
            {
                entity.Products.Add(request.Product);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
