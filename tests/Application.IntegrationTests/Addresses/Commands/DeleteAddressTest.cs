using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.DeleteAddress;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Addresses.Commands
{
    using static Testing;

    public class DeleteAddressTest : TestBase
    {
        [Test]
        public async Task ShouldRequireMinimumFields()
        {
            var command = new DeleteAddressCommand();

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldDeleteAddressAsync()
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

            var addressCommand = new CreateAddressCommand
            {
                Latitude = 48.5472M,
                Longitude = 72.1804M,
                CustomerId = customer.Id
            };

            var address = await SendAsync(addressCommand);

            await SendAsync(new DeleteAddressCommand
            {
                Id = address.Id
            });

            var a = await FindAsync<Address>(address.Id);
            a.Should().BeNull();
            customer.Should().NotBeNull();
        }
    }
}
