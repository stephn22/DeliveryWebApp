using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Riders.Commands.CreateRider;
using DeliveryWebApp.Application.Riders.Commands.DeleteRider;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Riders.Commands
{
    using static Testing;

    public class DeleteRiderTest : TestBase
    {
        [Test]
        public async Task ShouldRequireMinimumFields()
        {
            var command = new DeleteRiderCommand();

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldDeteleRiderAsync()
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

            var item = await SendAsync(command);

            await SendAsync(new DeleteRiderCommand
            {
                Id = item.Id
            });

            var rider = await FindAsync<Rider>(item.Id);
            rider.Should().BeNull();
            customer.Should().NotBeNull();
        }
    }
}
