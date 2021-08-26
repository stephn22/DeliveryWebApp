using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
using DeliveryWebApp.Application.BasketItems.Queries;
using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;
using DeliveryWebApp.Application.Baskets.Commands.UpdateBasket;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.OrderItems.Commands.CreateOrderItem;
using DeliveryWebApp.Application.OrderItems.Queries;
using DeliveryWebApp.Application.Orders.Commands.CreateOrder;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Domain.Constants;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.OrderItems.Commands
{
    using static Testing;

    public class CreateOrderItemTest
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateOrderItemCommand();

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateBasketItemAsync()
        {
            var user1 = await RunAsUserAsync("johnsmith@gmail.com", "Qwerty12!", Array.Empty<string>());

            var customer1 = await SendAsync(new CreateCustomerCommand
            {
                ApplicationUserFk = user1,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            });

            var user2 = await RunAsUserAsync("johndoe@gmail.com", "Qwerty12!", Array.Empty<string>());

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
                AddressLine1 = "Via Verdi",
                AddressLine2 = "",
                City = "Milan",
                Country = "Italy",
                PostalCode = "28100",
                StateProvince = "MI",
                Number = "2",
                Latitude = 48.5472M,
                Longitude = 72.1804M,
                CustomerId = customer1.Id
            };

            var address = await SendAsync(addressCommand);

            var product = await SendAsync(new CreateProductCommand
            {
                Image = null,
                Name = "Apple Pie",
                Category = ProductCategory.Dessert,
                Price = 4.99M,
                Discount = 0,
                Quantity = 23,
                RestaurateurId = restaurateur.Id
            });

            var b = await SendAsync(new CreateBasketCommand
            {
                Customer = customer1
            });

            var basket = await SendAsync(new UpdateBasketCommand
            {
                Basket = b,
                Product = product,
                Quantity = 1,
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

            var list = await SendAsync(new GetOrderItemsQuery
            {
                OrderId = order.Id
            });

            list.Should().NotBeNullOrEmpty();
            list.ForEach(o => o.Should().NotBeNull());
            list.ForEach(o => o.Id.Should().BeGreaterThan(0));
            list.ForEach(o => o.OrderId.Should().Be(order.Id));
            list.ForEach(o => o.ProductId.Should().Be(product.Id));
            list.ForEach(o => o.Quantity.Should().Be(1));
            list.ForEach(o => o.Discount.Should().Be(0));
            list.ForEach(o => o.ProductPrice.Should().Be(4.99M));
        }
    }
}