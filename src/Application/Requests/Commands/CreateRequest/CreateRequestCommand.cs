using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Requests.Commands.CreateRequest
{
    public class CreateRequestCommand : IRequest<Request>
    {
        public string Role { get; set; }
        public string Status { get; set; }
        public Customer Customer { get; set; }
    }

    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, Request>
    {
        private readonly IApplicationDbContext _context;

        public CreateRequestCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Request> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = new Request
            {
                Customer = request.Customer,
                Role = request.Role,
                Status = request.Status
            };

            _context.Requests.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
