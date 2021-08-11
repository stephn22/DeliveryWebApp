using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Orders.Commands.CreateOrder;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Domain.Constants;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Orders.Commands
{
    using static Testing;

    public class CreateOrderTest : TestBase
    {
        [Test]
        public async Task ShouldCreateOrderAsync()
        {
            var user1 = await RunAsDefaultUserAsync();

            var customer1 = await SendAsync(new CreateCustomerCommand
            {
                ApplicationUserFk = user1,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            });

            var user2 = await RunAsUserAsync("mariorossi@gmail.com", "Qwerty12!", Array.Empty<string>());

            var customer2 = await SendAsync(new CreateCustomerCommand
            {
                ApplicationUserFk = user2,
                FirstName = "Mario",
                LastName = "Rossi",
                Email = "mariorossi@gmail.com"
            });

            var restaurateur = await SendAsync(new CreateRestaurateurCommand
            {
                Customer = customer2
            });

            var orderCommand = new CreateOrderCommand
            {
                Customer = customer1,
                Restaurateur = restaurateur
            };

            var order = await SendAsync(orderCommand);

            order.Should().NotBeNull();
            order.Id.Should().BeGreaterThan(0);
            order.Status.Should().Be(OrderStatus.New);
            order.RestaurateurId.Should().Be(restaurateur.Id);
            order.CustomerId.Should().Be(customer1.Id);
            order.TotalPrice.Should().Be(0.00M);

            try
            {
                await SendAsync(new CreateOrderCommand
                {
                    Customer = customer2,
                    Restaurateur = restaurateur
                });
            }
            catch (ValidationException e)
            {
                Assert.Pass($"Exception thrown: {e.Message}");
            }
        }
    }
}
