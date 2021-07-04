using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommand : IRequest<Restaurant>
    {
        public byte[] Logo { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public Address Address { get; set; }
        public Restaurateur Restaurateur { get; set; }
    }

    public class CreateRestaurantCommandHandler : IRequestHandler<CreateRestaurantCommand, Restaurant>
    {
        private readonly IApplicationDbContext _context;

        public CreateRestaurantCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Restaurant> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var entity = new Restaurant
            {
                Address = request.Address,
                Category = request.Category,
                Logo = request.Logo,
                Name = request.Name,
                Restaurateur = request.Restaurateur,
                Products = new List<Product>(),
                Orders = new List<Order>()
            };

            _context.Restaurants.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
