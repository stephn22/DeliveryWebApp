using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Application.Restaurateurs.Commands.DeleteRestaurateur;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Domain.Constants;

namespace DeliveryWebApp.Application.IntegrationTests.Restaurateurs.Commands
{
    using static Testing;

    public class DeleteRestaurateurTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new DeleteRestaurateurCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldDeleteRestaurateurAsync()
        {
            var userId = await RunAsDefaultUserAsync();

            var customerCommand = new CreateCustomerCommand
            {
                ApplicationUserFk = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            };

            var customer = await SendAsync(customerCommand);

            var command = new CreateRestaurateurCommand
            {
                Customer = customer
            };

            var item = await SendAsync(command);

            var productCommand = new CreateProductCommand
            {
                Image = null,
                Name = "Pizza",
                Category = ProductCategory.Pizza,
                Price = 5.50M,
                Discount = 12,
                Quantity = 21,
                Restaurateur = item
            };

            var p = await SendAsync(productCommand);

            await SendAsync(new DeleteRestaurateurCommand
            {
                Id = item.Id
            });

            var product = await FindAsync<Product>(p.Id);

            var restaurateur = await FindAsync<Restaurateur>(item.Id);
            restaurateur.Should().BeNull();
            product.Should().BeNull();
            customer.Should().NotBeNull();
        }
    }
}
