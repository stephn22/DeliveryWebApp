using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Requests.Commands.CreateRequest;
using DeliveryWebApp.Application.Requests.Commands.DeleteRequest;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Requests.Commands
{
    using static Testing;

    public class DeleteRequestTest : TestBase
    {
        [Test]
        public async Task ShouldRequireMinimumFields()
        {
            var command = new DeleteRequestCommand();

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldDeleteRequestAsync()
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

            var request = await SendAsync(command);

            // delete request

            await SendAsync(new DeleteRequestCommand
            {
                Id = request.Id
            });

            var r = await FindAsync<Request>(request.Id);
            r.Should().BeNull();
            customer.Should().NotBeNull();
        }
    }
}
