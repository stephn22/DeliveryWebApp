using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;

namespace DeliveryWebApp.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommand : IRequest<int>
    {
        public string ApplicationUserFk { get; set; }

        public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, int>
        {
            private readonly IApplicationDbContext _context;

            public CreateCustomerCommandHandler(IApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<int> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
            {
                var entity = new Customer
                {
                    ApplicationUserFk = request.ApplicationUserFk
                };

                _context.Customers.Add(entity);

                await _context.SaveChangesAsync(cancellationToken);

                return entity.Id;
            }
        }
    }
}
