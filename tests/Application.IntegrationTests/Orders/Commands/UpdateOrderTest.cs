using AutoMapper.Internal;
using DeliveryWebApp.Application.Addresses.Commands.CreateAddress;
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
using NUnit.Framework;
using System;
using System.Linq;
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

            var addressCommand = new CreateAddressCommand
            {
                Latitude = 48.5472M,
                Longitude = 72.1804M,
                CustomerId = customer1.Id
            };

            var address = await SendAsync(addressCommand);

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
                RestaurateurId = restaurateur.Id
            });
            await SendAsync(new UpdateBasketCommand
            {
                Basket = basket,
                Product = p2,
                Quantity = 3,
                RestaurateurId = restaurateur.Id
            });
            await SendAsync(new UpdateBasketCommand
            {
                Basket = basket,
                Product = p3,
                Quantity = 1,
                RestaurateurId = restaurateur.Id
            });

            var updatedBasket = await FindAsync<Basket>(basket.Id);

            updatedBasket.BasketItems = await SendAsync(new GetBasketItemsQuery
            {
                Basket = updatedBasket
            });

            var createOrder = new CreateOrderCommand
            {
                Customer = customer1,
                Restaurateur = restaurateur,
                BasketItems = updatedBasket.BasketItems,
                AddressId = address.Id
            };

            var order = await SendAsync(createOrder);

            // update

            var updateOrder = new UpdateOrderCommand
            {
                DeliveryDate = new DateTime(2021, 10, 4, 14, 16, 23),
                Id = order.Id,
                OrderStatus = OrderStatus.Delivered
            };

            await SendAsync(updateOrder);

            order = await FindAsync<Order>(updateOrder.Id);

            order.Should().NotBeNull();
            order.Id.Should().BeGreaterThan(0);
            order.CustomerId.Should().Be(customer1.Id);
            order.RestaurateurId.Should().Be(restaurateur.Id);
            order.Date.Should().BeSameDateAs(DateTime.UtcNow);
            order.OrderItems = await SendAsync(new GetOrderItemsQuery
            {
                OrderId = order.Id
            });
            order.OrderItems.Should().NotBeNullOrEmpty();
            order.OrderItems.Select(o => o.OrderId).Should().Equal(order.Id);
            order.Status.Should().Be(OrderStatus.Delivered);
            order.DeliveryDate.Should().BeSameDateAs((DateTime)updateOrder.DeliveryDate);
            order.TotalPrice.Should().Be(63.37M);
        }
    }
}
