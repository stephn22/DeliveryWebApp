using AutoMapper.Internal;
using DeliveryWebApp.Application.BasketItems.Queries;
using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;
using DeliveryWebApp.Application.Baskets.Commands.UpdateBasket;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.OrderItems.Queries;
using DeliveryWebApp.Application.Orders.Commands.CreateOrder;
using DeliveryWebApp.Application.Orders.Commands.UpdateOrder;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using FluentAssertions.Common;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Orders.Commands
{
    using static Testing;

    public class UpdateOrderTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateOrderAsync()
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

            var p2 = await SendAsync(new CreateProductCommand
            {
                Image = null,
                Name = "Grilled Salmon",
                Category = ProductCategory.Fish,
                Price = 14.35M,
                Discount = 0,
                Quantity = 45,
                RestaurateurId = restaurateur.Id
            });

            var p3 = await SendAsync(new CreateProductCommand
            {
                Image = null,
                Name = "Apple Pie",
                Category = ProductCategory.Dessert,
                Price = 11.20M,
                Discount = 5,
                Quantity = 17,
                RestaurateurId = restaurateur.Id
            });

            // create a basket
            var basketCommand = new CreateBasketCommand
            {
                Customer = customer1
            };

            var basket = await SendAsync(basketCommand);

            // add products to basket
            await SendAsync(new UpdateBasketCommand
            {
                Basket = basket,
                Product = p1,
                Quantity = 2,
            });
            await SendAsync(new UpdateBasketCommand
            {
                Basket = basket,
                Product = p2,
                Quantity = 3
            });
            await SendAsync(new UpdateBasketCommand
            {
                Basket = basket,
                Product = p3,
                Quantity = 1
            });

            var updatedBasket = await FindAsync<Basket>(basket.Id);

            updatedBasket.BasketItems = await SendAsync(new GetBasketItemsQuery
            {
                Basket = updatedBasket
            });

            var createOrder = new CreateOrderCommand
            {
                Customer = customer1,
                Restaurateur = restaurateur
            };

            var order = await SendAsync(createOrder);

            // update order
            var command = new UpdateOrderCommand
            {
                Id = order.Id,
                BasketItems = updatedBasket.BasketItems,
                Date = DateTime.UtcNow,
                OrderStatus = OrderStatus.Checkout
            };

            await SendAsync(command);

            var update = await FindAsync<Order>(order.Id);

            update.Should().NotBeNull();
            update.Id.Should().BeGreaterThan(0);
            update.CustomerId.Should().Be(customer1.Id);
            update.RestaurateurId.Should().Be(restaurateur.Id);
            update.Date.Should().IsSameOrEqualTo(command.Date);
            update.OrderItems = await SendAsync(new GetOrderItemsQuery
            {
                OrderId = update.Id
            });
            update.OrderItems.Should().NotBeNullOrEmpty();
            update.OrderItems.ForAll(o => o.OrderId.Should().Be(update.Id));
            update.OrderItems.ForAll(o => o.Id.Should().BeGreaterThan(0));
            update.Status.Should().Be(command.OrderStatus);
            update.TotalPrice.Should().Be(63.37M);
        }
    }
}
