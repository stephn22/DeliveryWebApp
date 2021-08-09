using System.Threading.Tasks;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur;
using DeliveryWebApp.Domain.Constants;
using FluentAssertions;
using NUnit.Framework;

namespace DeliveryWebApp.Application.IntegrationTests.Restaurateurs.Commands
{
    using static Testing;
    public class UpdateRestaurateurTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateRestaurateurAsync()
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

            var restaurateurCommand = new CreateRestaurateurCommand
            {
                Customer = customer,
            };

            var restaurateur = await SendAsync(restaurateurCommand);

            var addressCommand = new CreateAddressCommand
            {
                AddressLine1 = "Via Verdi",
                AddressLine2 = "",
                City = "Milan",
                Country = "Italy",
                PostalCode = "28100",
                Number = "2",
                Latitude = 48.5472M,
                Longitude = 72.1804M,
                Restaurateur = restaurateur
            };

            var address = await SendAsync(addressCommand);

            var updateCommand = new UpdateRestaurateurCommand
            {
                Id = restaurateur.Id,
                Logo = new byte[2],
                RestaurantCategory = RestaurantCategory.Sushi,
                RestaurantAddress = address,
                RestaurantName = "Sushi 24/7"
            };

            var update = await SendAsync(updateCommand);

            update.Should().NotBeNull();
            update.Id.Should().NotBe(0);
            update.Id.Should().Be(restaurateur.Id);
            update.RestaurantName.Should().Be(updateCommand.RestaurantName);
            update.RestaurantCategory.Should().Be(updateCommand.RestaurantCategory);
            update.RestaurantAddressId.Should().Be(address.Id);
            update.Logo.Should().NotBeNull();
            update.Logo.Length.Should().Be(2);
        }
    }
}
