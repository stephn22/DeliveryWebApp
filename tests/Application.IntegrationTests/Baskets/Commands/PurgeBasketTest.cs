using DeliveryWebApp.Application.BasketItems.Queries;
using DeliveryWebApp.Application.Baskets.Commands.CreateBasket;
using DeliveryWebApp.Application.Baskets.Commands.PurgeBasket;
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

namespace DeliveryWebApp.Application.IntegrationTests.Baskets.Commands
{
    using static Testing;

    public class PurgeBasketTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new PurgeBasketCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

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
                Customer = customer
            };

            var basket = await SendAsync(basketCommand);

            // create basket items for each product

            await SendAsync(new UpdateBasketCommand
            {
                Basket = basket,
                Product = p1,
                Quantity = 1
            });
            await SendAsync(new UpdateBasketCommand
            {
                Basket = basket,
                Product = p2,
                Quantity = 2
            });
            var updatedBasket = await SendAsync(new UpdateBasketCommand
            {
                Basket = basket,
                Product = p3,
                Quantity = 3
            });

            await SendAsync(new PurgeBasketCommand
            {
                Id = basket.Id
            });

            var items = await SendAsync(new GetBasketItemsQuery
            {
                Basket = updatedBasket
            });

            var b = await FindAsync<Basket>(basket.Id);

            b.Should().NotBeNull();
            b.TotalPrice.Should().Be(0.00M);
            items.Should().BeNullOrEmpty();
        }
    }
}
