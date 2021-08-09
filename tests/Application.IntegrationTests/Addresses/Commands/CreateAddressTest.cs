using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using FluentAssertions;
using NUnit.Framework;

namespace DeliveryWebApp.Application.IntegrationTests.Addresses.Commands
{
    using static Testing;

    public class CreateAddressTest : TestBase
    {
        [Test]
        public async Task ShouldCreateAddressAsync()
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

            address.Should().NotBeNull();
            address.Id.Should().NotBe(0);
            address.AddressLine1.Should().Be(addressCommand.AddressLine1);
            address.AddressLine2.Should().Be(addressCommand.AddressLine2);
            address.City.Should().Be(addressCommand.City);
            address.Country.Should().Be(addressCommand.Country);
            address.PostalCode.Should().Be(addressCommand.PostalCode);
            address.Number.Should().Be(addressCommand.Number);
            address.Latitude.Should().Be(addressCommand.Latitude);
            address.StateProvince.Should().Be(addressCommand.StateProvince);
            address.Longitude.Should().Be(addressCommand.Longitude);
            address.CustomerId.Should().Be(customer.Id);
        }
    }
}
