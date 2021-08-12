using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Requests.Commands.CreateRequest;
using DeliveryWebApp.Application.Requests.Commands.UpdateRequest;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Requests.Commands
{
    using static Testing;

    public class UpdateRequestTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new UpdateRequestCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldUpdateRequestAsync()
        {
            // create customer first
            var userId = await RunAsDefaultUserAsync();

            var cmd = new CreateCustomerCommand
            {
                ApplicationUserFk = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            };

            var customer = await SendAsync(cmd);

            // then create request
            var command = new CreateRequestCommand
            {
                CustomerId = customer.Id,
                Role = RoleName.Restaurateur,
                Status = RequestStatus.Idle
            };

            var item = await SendAsync(command);

            var updateCommand = new UpdateRequestCommand
            {
                Id = item.Id,
                Status = RequestStatus.Accepted
            };

            await SendAsync(updateCommand);

            var update = await FindAsync<Request>(updateCommand.Id);

            update.Should().NotBeNull();
            update.Id.Should().BeGreaterThan(0);
            update.CustomerId.Should().Be(customer.Id);
            update.Role.Should().Be(command.Role);
            update.Status.Should().Be(updateCommand.Status);
        }
    }
}
