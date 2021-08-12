using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Riders.Commands.CreateRider
{
    public class CreateRiderCommand : IRequest<Rider>
    {
        public Customer Customer { get; set; }
        public decimal DeliveryCredit { get; set; }

        public class CreateRiderCommandHandler : IRequestHandler<CreateRiderCommand, Rider>
        {
            private readonly IApplicationDbContext _context;

            public CreateRiderCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<Rider> Handle(CreateRiderCommand request, CancellationToken cancellationToken)
            {
                var entity = new Rider
                {
                    CustomerId = request.Customer.Id,
                    DeliveryCredit = request.DeliveryCredit
                };

                _context.Riders.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return entity;
            }
        }
    }
}
