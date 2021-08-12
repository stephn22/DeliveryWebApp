using System;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DeliveryWebApp.Application.Requests.Commands.DeleteRequest;
using Microsoft.EntityFrameworkCore;

namespace DeliveryWebApp.Application.Requests.Commands.CreateRequest
{
    public class CreateRequestCommand : IRequest<Request>
    {
        public string Role { get; set; }
        public string Status { get; set; }
        public int CustomerId { get; set; }
    }

    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, Request>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateRequestCommandHandler(IApplicationDbContext context, IMapper mapper, IMediator mediator)
        {
            _context = context;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<Request> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Request>(request);

            // if customer has an old request, delete it
            try
            {
                var oldRequest =
                    await _context.Requests.FirstAsync(r => r.CustomerId == request.CustomerId, cancellationToken);

                await _mediator.Send(new DeleteRequestCommand
                {
                    Id = oldRequest.Id
                }, cancellationToken);
            }
            catch (InvalidOperationException)
            {
            }
            
            _context.Requests.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
