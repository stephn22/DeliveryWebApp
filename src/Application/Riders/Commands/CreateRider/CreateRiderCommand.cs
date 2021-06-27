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
        public double DeliveryCredit { get; set; }

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
                    Customer = request.Customer,
                    DeliveryCredit = request.DeliveryCredit
                };

                _context.Riders.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}
