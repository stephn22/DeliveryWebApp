using DeliveryWebApp.Application.BasketItems.Commands.CreateBasketItem;
using DeliveryWebApp.Application.BasketItems.Commands.DeleteBasketItem;
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

    public class DeleteBasketItemTest : TestBase
    {
        [Test]
        public async Task ShouldDeleteBasketItemAsync()
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
                Restaurateur = restaurateur
            };

            var product = await SendAsync(productCommand);

            // create basket
            var basketCommand = new CreateBasketCommand
            {
                Customer = customer
            };

            // finally create basket item
            var basket = await SendAsync(basketCommand);

            var command = new CreateBasketItemCommand
            {
                Basket = basket,
                Product = product,
                Quantity = 3
            };

            var item = await SendAsync(command);

            try
            {
                await SendAsync(new DeleteBasketItemCommand
                {
                    BasketItem = item
                });

                var basketItem = await FindAsync<BasketItem>(item.Id);
                basketItem.Should().BeNull();
            }
            catch (NotFoundException e)
            {
                Assert.Fail($"Expected no exception but got: {e.Message}");
            }
        }
    }
}
