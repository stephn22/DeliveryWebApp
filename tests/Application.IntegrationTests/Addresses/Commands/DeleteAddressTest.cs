using System.Threading.Tasks;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Addresses.Commands.DeleteAddress;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeliveryWebApp.Application.IntegrationTests.Addresses.Commands
{
    using static Testing;

    public class DeleteAddressTest : TestBase
    {
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
