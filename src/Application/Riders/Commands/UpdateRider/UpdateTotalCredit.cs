using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Riders.Commands.UpdateRider
{
    public class UpdateTotalCredit : IRequest<Rider>
    {
        /// <summary>
        /// Rider id
        /// </summary>
        public int Id { get; set; }
    }

    public class UpdateTotalCreditHandler : IRequestHandler<UpdateTotalCredit, Rider>
    {
        private readonly IApplicationDbContext _context;

        public UpdateTotalCreditHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Rider> Handle(UpdateTotalCredit request, CancellationToken cancellationToken)
        {
            var entity = await _context.Riders.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Rider), request.Id);
            }

            entity.TotalCredit += entity.DeliveryCredit;

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
