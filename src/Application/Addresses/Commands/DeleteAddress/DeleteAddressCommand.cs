using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Addresses.Commands.DeleteAddress
{
    public class DeleteAddressCommand : IRequest<Address>
    {
        /// <summary>
        /// Address id
        /// </summary>
        public int Id { get; set; }
    }

    public class DeleteAddressCommandHandler : IRequestHandler<DeleteAddressCommand, Address>
    {
        private readonly IApplicationDbContext _context;

        public DeleteAddressCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Address> Handle(DeleteAddressCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Addresses.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Address), request.Id);
            }

            _context.Addresses.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}
