using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Requests.Commands.UpdateRequest
{
    public class UpdateRequestCommand : IRequest<Request>
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }

    public class UpdateRequestCommandHandler : IRequestHandler<UpdateRequestCommand, Request>
    {
        private readonly IApplicationDbContext _context;

        public UpdateRequestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Request> Handle(UpdateRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Requests.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Request), request.Id);
            }

            entity.Status = request.Status;

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
