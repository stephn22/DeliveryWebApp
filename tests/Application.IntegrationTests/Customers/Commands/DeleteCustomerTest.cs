using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Customers.Commands.DeleteCustomer;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Application.Riders.Commands.CreateRider;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Customers.Commands
{
    using static Testing;

    public class DeleteCustomerTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new DeleteCustomerCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldDeleteCustomerAsync()
        {
            var userId = await RunAsAdministratorAsync();

            var create = new CreateCustomerCommand
            {
                ApplicationUserFk = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            };

            var item = await SendAsync(create);

            await SendAsync(new DeleteCustomerCommand
            {
                Customer = item
            });

            var customer = await FindAsync<Customer>(item.Id);
            customer.Should().BeNull();
        }

        [Test]
        public async Task ShouldDeleteCustomerAndBasketAsync()
        {
            var userId = await RunAsAdministratorAsync();

            var create = new CreateCustomerCommand
            {
                ApplicationUserFk = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            };

            var item = await SendAsync(create);

            var basketCommand = new CreateBasketCommand
            {
                Customer = item
            };

            var b = await SendAsync(basketCommand);

            await SendAsync(new DeleteCustomerCommand
            {
                Customer = item
            });

            var customer = await FindAsync<Customer>(item.Id);
            var basket = await FindAsync<Basket>(b.Id);

            customer.Should().BeNull();
            basket.Should().BeNull();
        }

        [Test]
        public async Task ShouldDeleteCustomerAndRiderAsync()
        {
            var userId = await RunAsDefaultUserAsync();

            var customerCommand = new CreateCustomerCommand
            {
                ApplicationUserFk = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            };

            var c = await SendAsync(customerCommand);

            var command = new CreateRiderCommand
            {
                Customer = c,
                DeliveryCredit = 12.52M
            };

            var r = await SendAsync(command);

            await SendAsync(new DeleteCustomerCommand
            {
                Customer = c
            });

            var customer = await FindAsync<Customer>(c.Id);
            var rider = await FindAsync<Rider>(r.Id);

            customer.Should().BeNull();
            rider.Should().BeNull();
        }

        [Test]
        public async Task ShouldDeleteCustomerAndRestaurateurAsync()
        {
            var userId = await RunAsDefaultUserAsync();

            var customerCommand = new CreateCustomerCommand
            {
                ApplicationUserFk = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            };

            var c = await SendAsync(customerCommand);

            var command = new CreateRestaurateurCommand
            {
                Customer = c
            };

            var r = await SendAsync(command);

            await SendAsync(new DeleteCustomerCommand
            {
                Customer = c
            });

            var customer = await FindAsync<Customer>(c.Id);
            var restaurateur = await FindAsync<Restaurateur>(r.Id);

            customer.Should().BeNull();
            restaurateur.Should().BeNull();
        }
    }
}
