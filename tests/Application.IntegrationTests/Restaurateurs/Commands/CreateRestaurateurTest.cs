using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Restaurateurs.Commands
{
    using static Testing;

    public class CreateRestaurateurTest : TestBase
    {
        private static Customer Customer => new()
        {
            ApplicationUserFk = "application-user-fk"
        };

        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateRestaurateurCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateRestaurateurAsync()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new CreateRestaurateurCommand
            {
                Customer = Customer
            };

            var itemId = await SendAsync(command);
            var restaurateur = await FindAsync<Restaurateur>(itemId);

            restaurateur.Should().NotBeNull();
            restaurateur.Should().Be(command.Customer);
            restaurateur.RestaurantName.Should().BeNull();
            restaurateur.RestaurantAddress.Should().BeNull();
            restaurateur.RestaurantCategory.Should().BeNull();
        }
    }
}
