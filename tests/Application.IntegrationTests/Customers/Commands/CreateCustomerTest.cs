using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Customers.Commands
{
    using static Testing;
    public class CreateCustomerTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateCustomerCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateCustomerAsync()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new CreateCustomerCommand
            {
                ApplicationUserFk = "application-user-fk"
            };



            var itemId = await SendAsync(command);

            var customer = await FindAsync<Customer>(itemId);

            customer.Should().NotBeNull();
            customer.ApplicationUserFk.Should().Be(command.ApplicationUserFk);
        }
    }
}
