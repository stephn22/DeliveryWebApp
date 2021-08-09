using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Application.Restaurateurs.Commands.DeleteRestaurateur;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeliveryWebApp.Application.IntegrationTests.Restaurateurs.Commands
{
    using static Testing;

    public class DeleteRestaurateurTest : TestBase
    {
        [Test]
        public async Task ShouldDeleteRestaurateurAsync()
        {
            var userId = await RunAsDefaultUserAsync();

            var customerCommand = new CreateCustomerCommand
            {
                ApplicationUserFk = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            };

            var customer = await SendAsync(customerCommand);

            var command = new CreateRestaurateurCommand
            {
                Customer = customer
            };

            var item = await SendAsync(command);

            await SendAsync(new DeleteRestaurateurCommand
            {
                Id = item.Id
            });

            var restaurateur = await FindAsync<Restaurateur>(item.Id);
            restaurateur.Should().BeNull();
            customer.Should().NotBeNull();
        }
    }
}
