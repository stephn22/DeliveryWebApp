using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Products.Commands.DeleteProduct;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeliveryWebApp.Application.IntegrationTests.Products.Commands
{
    using static Testing;
    public class DeleteProductTest : TestBase
    {
        [Test]
        public async Task ShouldDeleteProductAsync()
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

            var item = await SendAsync(productCommand);

            await SendAsync(new DeleteProductCommand
            {
                Product = item
            });

            var product = await FindAsync<Product>(item.Id);
            product.Should().BeNull();
        }
    }
}
