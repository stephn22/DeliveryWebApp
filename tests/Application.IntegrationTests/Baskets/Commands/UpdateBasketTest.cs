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

            var updateCommand = new UpdateBasketCommand
            {
                Basket = basket,
                Product = p1,
                Quantity = 3
            };

            var update = await SendAsync(updateCommand);

            var product = await FindAsync<Product>(p1.Id);

            update.Should().NotBeNull();
            update.Id.Should().Be(basket.Id);
            product.Quantity.Should().Be(18);
            update.TotalPrice.Should().Be(14.52M).And.BeOfType(typeof(decimal));
            update.CustomerId.Should().Be(customer.Id);
        }
    }
}
