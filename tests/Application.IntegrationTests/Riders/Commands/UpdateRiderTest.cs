using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Riders.Commands.CreateRider;
using DeliveryWebApp.Application.Riders.Commands.UpdateRider;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Riders.Commands
{
    using static Testing;

    public class UpdateRiderTest : TestBase
    {
        [Test]
        public async Task ShouldRequireMinimumFields()
        {
            var command = new UpdateRiderCommand();

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldUpdateRiderAsync()
        {
            var userId = await RunAsDefaultUserAsync();

            var customerCommand = new CreateCustomerCommand
            {
                ApplicationUserFk = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            };

            var customer = await SendAsync(customerCommand);

            var command = new CreateRiderCommand
            {
                Customer = customer,
                DeliveryCredit = 12.52M
            };

            var rider = await SendAsync(command);

            var updateCommand = new UpdateRiderCommand
            {
                Id = rider.Id,
                DeliveryCredit = 11.75M
            };

            var update = await SendAsync(updateCommand);

            update.Should().NotBeNull();
            update.Id.Should().NotBe(0);
            update.Id.Should().Be(rider.Id);
            update.CustomerId.Should().Be(customer.Id);
            update.DeliveryCredit.Should().Be(updateCommand.DeliveryCredit);
        }
    }
}
