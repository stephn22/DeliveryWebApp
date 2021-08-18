using System;
using System.Linq;
using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;
using DeliveryWebApp.Application.Baskets.Commands.UpdateBasket;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using DeliveryWebApp.Application.BasketItems.Queries;

namespace DeliveryWebApp.Application.IntegrationTests.Baskets.Commands
{
    using static Testing;

    public class UpdateBasketTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new UpdateBasketCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldUpdateBasketAsync()
        {
            var userId = await RunAsDefaultUserAsync();

            var customerCommand = new CreateCustomerCommand()
            {
                ApplicationUserFk = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            };

            var customer = await SendAsync(customerCommand);

            var basketCommand = new CreateBasketCommand
            {
                Customer = customer
            };

            var basket = await SendAsync(basketCommand);

            var restaurateurCommand = new CreateRestaurateurCommand
            {
                Customer = customer
            };

            var restaurateur1 = await SendAsync(restaurateurCommand);

            // create some products

            var p1 = await SendAsync(new CreateProductCommand
            {
                Image = null,
                Name = "Pizza",
                Category = ProductCategory.Pizza,
                Price = 5.50M,
                Discount = 12,
                Quantity = 21,
                RestaurateurId = restaurateur1.Id
            });

            var updateCommand = new UpdateBasketCommand
            {
                Basket = basket,
                Product = p1,
                Quantity = 3,
                RestaurateurId = restaurateur1.Id
            };

            var update = await SendAsync(updateCommand);

            var product = await FindAsync<Product>(p1.Id);

            update.Should().NotBeNull();
            update.Id.Should().Be(basket.Id);
            product.Quantity.Should().Be(p1.Quantity);
            update.TotalPrice.Should().Be(14.52M).And.BeOfType(typeof(decimal));
            update.CustomerId.Should().Be(customer.Id);

            var updateCommand2 = new UpdateBasketCommand
            {
                Basket = basket,
                Product = p1,
                Quantity = 5,
                RestaurateurId = restaurateur1.Id
            };

            var update2 = await SendAsync(updateCommand2);

            var items = await SendAsync(new GetBasketItemsQuery
            {
                Basket = basket
            });

            items.Should().NotBeNullOrEmpty();

            try
            {
                var item = items.First(i => i.ProductId == p1.Id);

                item.BasketId.Should().Be(basket.Id);
                item.ProductId.Should().Be(p1.Id);
                item.Quantity.Should().Be(8);
            }
            catch (InvalidOperationException)
            {
                Assert.Fail();
            }

            var user2 = await RunAsUserAsync("mariorossi@gmail.com", "Qwerty12!", Array.Empty<string>());

            var customer2 = await SendAsync(new CreateCustomerCommand
            {
                ApplicationUserFk = user2,
                FirstName = "Mario",
                LastName = "Rossi",
                Email = "mariorossi@gmail.com"
            });

            // try adding a product from a different restaurateur

            var restaurateur2 = await SendAsync(new CreateRestaurateurCommand
            {
                Customer = customer2
            });

            var p2 = await SendAsync(new CreateProductCommand
            {
                Image = null,
                Name = "Grilled Salmon",
                Category = ProductCategory.Fish,
                Price = 14.35M,
                Discount = 0,
                Quantity = 45,
                RestaurateurId = restaurateur2.Id
            });

            var updateCommand3 = new UpdateBasketCommand
            {
                Basket = basket,
                Product = p2,
                Quantity = 2,
                RestaurateurId = restaurateur2.Id
            };

            var update3 = await SendAsync(updateCommand3);

            update3.Should().NotBeNull();
            update3.Id.Should().BeGreaterThan(0);
            update3.TotalPrice.Should().Be(p2.Price * 2);

            var basketItems = await SendAsync(new GetBasketItemsQuery
            {
                Basket = basket
            });

            basketItems.Should().NotBeNullOrEmpty();
            basketItems.Count.Should().Be(1);
            basketItems[0].ProductId.Should().Be(p2.Id);
            basketItems[0].Quantity.Should().Be(updateCommand3.Quantity);
        }
    }
}
