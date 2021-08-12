using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

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
        private readonly IMapper _mapper;

        public CreateRequestCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Request> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = new Request // TODO: try with mediator
            {
                CustomerId = request.Customer.Id,
                Role = request.Role,
                Status = request.Status
            };

            _context.Requests.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
