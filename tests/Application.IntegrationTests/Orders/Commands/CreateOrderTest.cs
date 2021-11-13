using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.BasketItems.Queries;
using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;
using DeliveryWebApp.Application.Baskets.Commands.UpdateBasket;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Orders.Commands.CreateOrder;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
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
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateOrderCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<NullReferenceException>();
        }

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

            var addressCommand = new CreateAddressCommand
            {
                Latitude = 48.5472M,
                Longitude = 72.1804M,
                CustomerId = customer1.Id
            };

            var address = await SendAsync(addressCommand);

            var basketCommand = new CreateBasketCommand
            {
                Customer = customer1
            };

            var basket = await SendAsync(basketCommand);

            var p1 = await SendAsync(new CreateProductCommand
            {
                Image = null,
                Name = "Pizza",
                Category = ProductCategory.Pizza,
                Price = 5.50M,
                Discount = 12,
                Quantity = 21,
                RestaurateurId = restaurateur.Id
            });

            await SendAsync(new UpdateBasketCommand
            {
                Basket = basket,
                Product = p1,
                Quantity = 3,
                RestaurateurId = restaurateur.Id
            });

            basket.BasketItems = await SendAsync(new GetBasketItemsQuery
            {
                Basket = basket
            });

            var orderCommand = new CreateOrderCommand
            {
                Customer = customer1,
                Restaurateur = restaurateur,
                BasketItems = basket.BasketItems,
                AddressId = address.Id
            };

            var order = await SendAsync(orderCommand);

            order.Should().NotBeNull();
            order.Id.Should().BeGreaterThan(0);
            order.Status.Should().Be(OrderStatus.New);
            order.RestaurateurId.Should().Be(restaurateur.Id);
            order.CustomerId.Should().Be(customer1.Id);
            order.TotalPrice.Should().Be(14.52M);

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
