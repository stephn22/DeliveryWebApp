using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.UpdateAddress;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using NUnit.Framework;

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
        }
    }
}
