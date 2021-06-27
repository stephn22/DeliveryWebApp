using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur
{
    public class CreateRestaurateurCommand : IRequest<int>
    {
        public Customer Customer { get; set; }

        public class CreateRestaurateurCommandHandler : IRequestHandler<CreateRestaurateurCommand, int>
        {
            private readonly IApplicationDbContext _context;

            public CreateRestaurateurCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateRestaurateurCommand request, CancellationToken cancellationToken)
            {
                var entity = new Restaurateur
                {
                    Customer = request.Customer
                };

                _context.Restaurateurs.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}
