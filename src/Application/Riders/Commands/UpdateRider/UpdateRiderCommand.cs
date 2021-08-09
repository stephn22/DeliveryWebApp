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

namespace DeliveryWebApp.Application.Riders.Commands.UpdateRider
{
    public class UpdateRiderCommand : IRequest<Rider>
    {
        public int Id { get; set; }
        public decimal DeliveryCredit { get; set; }
    }

    public class UpdateRiderCommandHandler : IRequestHandler<UpdateRiderCommand, Rider>
    {
        private readonly IApplicationDbContext _context;

        public UpdateRiderCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Rider> Handle(UpdateRiderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Riders.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Rider), request.Id);
            }

            entity.DeliveryCredit = request.DeliveryCredit;

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
