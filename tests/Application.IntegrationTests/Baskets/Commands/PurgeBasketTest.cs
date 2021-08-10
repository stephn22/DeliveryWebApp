using DeliveryWebApp.Application.BasketItems.Commands.CreateBasketItem;
using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;
using DeliveryWebApp.Application.Baskets.Commands.PurgeBasket;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Baskets.Commands
{
    using static Testing;

    public class PurgeBasketTest : TestBase
    {
        [Test]
        public async Task ShouldPurgeBasketAsync()
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

            // create some products

            var p1 = await SendAsync(new CreateProductCommand
            {
                Image = null,
                Name = "Pizza",
                Category = ProductCategory.Pizza,
                Price = 5.50M,
                Discount = 12,
                Quantity = 21,
                Restaurateur = restaurateur
            });

            var p2 = await SendAsync(new CreateProductCommand
            {
                Image = null,
                Name = "Grilled Salmon",
                Category = ProductCategory.Fish,
                Price = 14.35M,
                Discount = 0,
                Quantity = 45,
                Restaurateur = restaurateur
            });

            var p3 = await SendAsync(new CreateProductCommand
            {
                Image = null,
                Name = "Apple Pie",
                Category = ProductCategory.Dessert,
                Price = 11.20M,
                Discount = 5,
                Quantity = 17,
                Restaurateur = restaurateur
            });

            // create a basket
            var basketCommand = new CreateBasketCommand
            {
                Customer = customer
            };

            var basket = await SendAsync(basketCommand);

            // create basket items for each product

            var b1 = await SendAsync(new CreateBasketItemCommand
            {
                Basket = basket,
                Product = p1,
                Quantity = 1
            });

            var b2 = await SendAsync(new CreateBasketItemCommand
            {
                Basket = basket,
                Product = p2,
                Quantity = 2
            });

            var b3 = await SendAsync(new CreateBasketItemCommand
            {
                Basket = basket,
                Product = p3,
                Quantity = 3
            });

            await SendAsync(new PurgeBasketCommand
            {
                Basket = basket
            });

            var item1 = await FindAsync<BasketItem>(b1.Id);
            var item2 = await FindAsync<BasketItem>(b2.Id);
            var item3 = await FindAsync<BasketItem>(b3.Id);

            basket.Should().NotBeNull();
            item1.Should().BeNull();
            item2.Should().BeNull();
            item3.Should().BeNull();
        }
    }
}
