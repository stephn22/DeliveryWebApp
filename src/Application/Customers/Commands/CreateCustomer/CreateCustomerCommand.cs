using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;

namespace DeliveryWebApp.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<Customer>
    {
        public string ApplicationUserFk { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Customer>
        {
            private readonly IApplicationDbContext _context;
            private readonly IMediator _mediator;

            public CreateCustomerCommandHandler(IApplicationDbContext context, IMediator mediator)
            {
                _context = context;
                _mediator = mediator;
            }

            public async Task<Customer> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                var entity = new Customer
                {
                    ApplicationUserFk = request.ApplicationUserFk,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email
                };

                var basket = await _mediator.Send(new CreateBasketCommand
                {
                    Customer = entity
                });

                entity.BasketId = basket.Id;

                _context.Customers.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return entity;
            }
        }
    }
}
