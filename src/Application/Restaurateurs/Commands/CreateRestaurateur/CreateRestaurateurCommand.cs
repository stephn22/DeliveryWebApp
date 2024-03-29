﻿using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur
{
    public class CreateRestaurateurCommand : IRequest<Restaurateur>
    {
        public Customer Customer { get; set; }

        public class CreateRestaurateurCommandHandler : IRequestHandler<CreateRestaurateurCommand, Restaurateur>
        {
            private readonly IApplicationDbContext _context;

            public CreateRestaurateurCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Restaurateur> Handle(CreateRestaurateurCommand request, CancellationToken cancellationToken)
            {
                var entity = new Restaurateur
                {
                    CustomerId = request.Customer.Id
                };

                _context.Restaurateurs.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return entity;
            }
        }
    }
}
