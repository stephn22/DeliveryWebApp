using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Requests.Commands.DeleteRequest
{
    public class DeleteRequestCommand : IRequest<Request>
    {
        public Request Request { get; set; }
    }

    public class DeleteRequestCommandHandler : IRequestHandler<DeleteRequestCommand, Request>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DeleteRequestCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Request> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Request>(request.Request);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Request), request.Request);
            }

            _context.Requests.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
