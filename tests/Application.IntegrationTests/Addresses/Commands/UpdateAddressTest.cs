using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Addresses.Commands
{
    using static Testing;

    public class UpdateAddressTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new UpdateAddressCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldUpdateAddressAsync()
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

            const string newPlaceName = "Via Verdi, Palazzo Tartara, 2, Milan, MI, 28100, Italy";

            var updateCommand = new UpdateAddressCommand
            {
                Id = address.Id,
                PlaceName = newPlaceName,
                Latitude = 23.4535M,
                Longitude = 15.7628M,
            };

            await SendAsync(updateCommand);

            var update = await FindAsync<Address>(address.Id);

            update.Should().NotBeNull();
            update.Id.Should().BeGreaterThan(0);
            update.PlaceName.Should().Be(updateCommand.PlaceName);
            update.Latitude.Should().Be(updateCommand.Latitude);
            update.Longitude.Should().Be(updateCommand.Longitude);
        }
    }
}
