using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using FluentAssertions;
using NUnit.Framework;

namespace DeliveryWebApp.Application.IntegrationTests.Products.Commands
{
    using static Testing;

    public class UpdateProductTest : TestBase
    {
        [Test]
        public async Task ShouldUpdateProductAsync()
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

            var restaurateurCommand = new CreateRestaurateurCommand
            {
                Customer = customer
            };

            var restaurateur = await SendAsync(restaurateurCommand);

            var productCommand = new CreateProductCommand
            {
                Image = null,
                Name = "Pizza",
                Category = "Pizza",
                Price = 5.50M,
                Discount = 12,
                Quantity = 21,
                Restaurateur = restaurateur
            };

            var product = await SendAsync(productCommand);

            var updateCommand = new UpdateProductCommand
            {
                Id = product.Id,
                Image = product.Image,
                Name = "French Fries",
                Category = "Snacks",
                Discount = 0,
                Price = 7.50M,
            };

            var update = await SendAsync(updateCommand);

            update.Should().NotBeNull();
            update.Id.Should().Be(product.Id);
            update.Name.Should().Be(updateCommand.Name);
            update.Image.Should().BeEquivalentTo(product.Image);
            update.Category.Should().Be(updateCommand.Category);
            update.Price.Should().Be(updateCommand.Price);
            update.Price.Should().BeOfType(typeof(decimal));
            update.Discount.Should().Be(updateCommand.Discount);
            update.Quantity.Should().Be(product.Quantity);
        }
    }
}
