using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;

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
            restaurateur.Restaurant.Should().BeNull();
            restaurateur.Reviews.Should().BeNullOrEmpty();
        }
    }
}
