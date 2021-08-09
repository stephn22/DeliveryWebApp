using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Customers.Commands.UpdateCustomer;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Customers.Commands
{
    using static Testing;

    public class UpdateCustomerTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateCustomerAsync()
        {
            var userId = await RunAsDefaultUserAsync();

            var customerCommand = new CreateCustomerCommand()
            {
                ApplicationUserFk = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            };

            var customer = await SendAsync(customerCommand);

            var updateCommand = new UpdateCustomerCommand
            {
                Id = customer.Id,
                FName = "Mario",
                LName = "Rossi"
            };

            var update = await SendAsync(updateCommand);

            update.Should().NotBeNull();
            update.Id.Should().Be(customer.Id);
            update.ApplicationUserFk.Should().Be(customer.ApplicationUserFk);
            update.FirstName.Should().Be(updateCommand.FName);
            update.LastName.Should().Be(updateCommand.LName);
            update.Email.Should().Be(customer.Email);
        }
    }
}
