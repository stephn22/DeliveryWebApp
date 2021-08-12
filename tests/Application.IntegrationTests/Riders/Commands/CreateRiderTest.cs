using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Riders.Commands.CreateRider;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Riders.Commands
{
    using static Testing;

    public class CreateRiderTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateRiderCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateRiderAsync()
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

            rider.Should().NotBeNull();
            rider.Id.Should().NotBe(0);
            rider.DeliveryCredit.Should().Be(command.DeliveryCredit);
            rider.CustomerId.Should().Be(customer.Id);
        }
    }
}
