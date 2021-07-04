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

namespace DeliveryWebApp.Application.Requests.Commands.DeleteRequest
{
    public class DeleteRequestCommand : IRequest<Request>
    {
        public int Id { get; set; }
    }

    public class DeleteRequestCommandHandler : IRequestHandler<DeleteRequestCommand, Request>
    {
        private readonly IApplicationDbContext _context;

        public DeleteRequestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Request> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Requests.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Request), request.Id);
            }

            _context.Requests.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
