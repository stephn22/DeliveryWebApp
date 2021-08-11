using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using NUnit.Framework;
using System.Threading.Tasks;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;

namespace DeliveryWebApp.Application.IntegrationTests.Addresses.Commands
{
    using static Testing;

    public class UpdateAddressTest : TestBase
    {
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
                AddressLine1 = "Via Verdi",
                AddressLine2 = "",
                City = "Milan",
                Country = "Italy",
                PostalCode = "28100",
                StateProvince = "MI",
                Number = "2",
                Latitude = 48.5472M,
                Longitude = 72.1804M,
                Customer = customer
            };

            var address = await SendAsync(addressCommand);

            var updateCommand = new UpdateAddressCommand
            {
                Id = address.Id,
                AddressLine2 = "Palazzo Tartara",
                PostalCode = "18200",
                Number = "13",
                Latitude = 23.4535M,
                Longitude = 15.7628M,
            };
            
            await SendAsync(updateCommand); // FIXME: validation errors

            var update = await FindAsync<Address>(address.Id);

            update.Should().NotBeNull();
            update.Id.Should().BeGreaterThan(0);
            update.AddressLine1.Should().Be(address.AddressLine1);
            update.AddressLine2.Should().Be(updateCommand.AddressLine2);
            update.City.Should().Be(address.City);
            update.Number.Should().Be(updateCommand.Number);
            update.PostalCode.Should().Be(updateCommand.PostalCode);
            update.StateProvince.Should().Be(address.StateProvince);
            update.Country.Should().Be(address.Country);
            update.Latitude.Should().Be(updateCommand.Latitude);
            update.Longitude.Should().Be(updateCommand.Longitude);
        }
    }
}
