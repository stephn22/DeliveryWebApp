using System;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace DeliveryWebApp.Application.Customers.Commands.DeleteCustomer
{
    public class DeleteCustomerCommand : IRequest<Customer>
    {
        public Customer Customer { get; set; }
    }

    public class DeleteCustomerCommandHandler : IRequestHandler<DeleteCustomerCommand, Customer>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<DeleteCustomerCommand> _logger;

        public DeleteCustomerCommandHandler(IApplicationDbContext context, IMapper mapper, ILogger<DeleteCustomerCommand> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Customer> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Customer>(request.Customer);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Customer), request.Customer.Id);
            }

            // search if customer is also a restaurateur or a rider and delete it
            try
            {
                var restaurateur =
                    await _context.Restaurateurs.FirstAsync(r => r.CustomerId == entity.Id, cancellationToken);

                _context.Restaurateurs.Remove(restaurateur);
                _logger.LogInformation($"Deleted restaurateur with id '{restaurateur.Id}");
            }
            catch (InvalidOperationException)
            {
            }
            try
            {
                var rider =
                    await _context.Riders.FirstAsync(r => r.CustomerId == entity.Id, cancellationToken);

                _context.Riders.Remove(rider);
                _logger.LogInformation($"Deleted rider with id '{rider.Id}");
            }
            catch (InvalidOperationException)
            {
            }
            
            var addresses = await _context.Addresses.Where(a => a.CustomerId == entity.Id)
                .ToListAsync(cancellationToken);

            if (addresses.Count != 0)
            {
                _context.Addresses.RemoveRange(addresses);
            } 
            _context.Customers.Remove(entity);
            _logger.LogInformation($"Deleted customer with id '{entity.Id}");

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }
    }
}