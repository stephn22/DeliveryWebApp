using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Requests.Commands.CreateRequest;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Security;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Requests.Commands
{
    using static Testing;

    public class CreateRequestTest : TestBase
    {
        //[Test]
        //public void ShouldRequireMinimumFields()
        //{
        //    var command = new CreateCustomerCommand();

        //    FluentActions.Invoking(() =>
        //        SendAsync(command)).Should().Throw<ValidationException>();
        //}

        [Test]
        public async Task ShouldCreateRequestAsync()
        {
            // create customer first
            var userId = await RunAsDefaultUserAsync();

            var cmd = new CreateCustomerCommand
            {
                ApplicationUserFk = userId
            };

            var customer = await SendAsync(cmd);

            // then create request
            var command = new CreateRequestCommand
            {
                Customer = customer,
                Role = RoleName.Restaurateur,
                Status = RequestStatus.Idle
            };

            var item = await SendAsync(command);

            var request = await FindAsync<Request>(item.Id);

            request.Should().NotBeNull();
            request.CustomerId.Should().Be(customer.Id);
            request.Status.Should().Be(command.Status);
            request.Role.Should().Be(command.Role);
            request.Customer.Should().BeNull();
        }
    }
}
