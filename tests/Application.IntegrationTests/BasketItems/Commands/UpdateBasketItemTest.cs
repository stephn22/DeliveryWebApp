﻿using DeliveryWebApp.Application.BasketItems.Commands.CreateBasketItem;
using DeliveryWebApp.Application.BasketItems.Commands.UpdateBasketItem;
using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.BasketItems.Commands
{
    using static Testing;

    public class UpdateBasketItemTest : TestBase
    {
        [Test]
        public async Task ShouldRequireMinimumFields()
        {
            var command = new UpdateBasketItemCommand();

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }

        [Test]
        public async Task ShouldUpdateBasketItemAsync()
        {
            var userId = await RunAsDefaultUserAsync();

            // create customer
            var customerCommand = new CreateCustomerCommand()
            {
                ApplicationUserFk = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            };

            var customer = await SendAsync(customerCommand);

            // create restaurateur
            var restaurateurCommand = new CreateRestaurateurCommand
            {
                Customer = customer
            };

            var restaurateur = await SendAsync(restaurateurCommand);

            // create product
            var productCommand = new CreateProductCommand
            {
                Image = null,
                Name = "Pizza",
                Category = ProductCategory.Pizza,
                Price = 5.50M,
                Discount = 12,
                Quantity = 21,
                RestaurateurId = restaurateur.Id
            };

            var product = await SendAsync(productCommand);

            // create basket
            var basketCommand = new CreateBasketCommand
            {
                Customer = customer
            };

            // create basket item
            var basket = await SendAsync(basketCommand);

            var command = new CreateBasketItemCommand
            {
                Basket = basket,
                Product = product,
                Quantity = 3
            };

            var basketItem = await SendAsync(command);

            var newProductCommand = new CreateProductCommand
            {
                Image = null,
                Name = "Hamburger",
                Category = ProductCategory.Hamburger,
                Price = 7.95M,
                Discount = 15,
                Quantity = 34,
                RestaurateurId = restaurateur.Id
            };

            var newProduct = await SendAsync(newProductCommand);

            var updateBasketItem = new UpdateBasketItemCommand
            {
                Id = basketItem.Id,
                Quantity = newProductCommand.Quantity
            };

            await SendAsync(updateBasketItem);

            var update = await FindAsync<BasketItem>(updateBasketItem.Id);
            var b = await FindAsync<Basket>(basket.Id);

            update.Should().NotBeNull();
            update.Id.Should().NotBe(0);
            update.Id.Should().Be(basketItem.Id);
            update.ProductId.Should().Be(product.Id);
            update.Quantity.Should().Be(updateBasketItem.Quantity);
            update.BasketId.Should().Be(basket.Id);
            b.TotalPrice.Should().Be(164.56M);
        }
    }
}
