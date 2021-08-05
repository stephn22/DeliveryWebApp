using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Riders.Commands.CreateRider
{
    public class CreateRiderCommand : IRequest<int>
    {
        public Customer Customer { get; set; }
        public decimal DeliveryCredit { get; set; }

        public class CreateRiderCommandHandler : IRequestHandler<CreateRiderCommand, int>
        {
            private readonly IApplicationDbContext _context;

            public CreateRiderCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateRiderCommand request, CancellationToken cancellationToken)
            {
                var entity = new Rider
                {
                    ApplicationUserFk = request.Customer.ApplicationUserFk,
                    Addresses = request.Customer.Addresses,
                    Basket = request.Customer.Basket,
                    Email = request.Customer.Email,
                    FirstName = request.Customer.FirstName,
                    LastName = request.Customer.LastName,
                    Orders = request.Customer.Orders,
                    DeliveryCredit = request.DeliveryCredit
                };

                _context.Customers.Remove(request.Customer);
                _context.Riders.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}
