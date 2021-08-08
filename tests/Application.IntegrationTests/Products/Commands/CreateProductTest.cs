using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Products.Commands
{
    using static Testing;
    public class CreateProductTest : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateProductCommand();

            FluentActions.Invoking(() =>
                SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldCreateProductAsync()
        {
            var userId = await RunAsDefaultUserAsync();

            var customerCommand = new CreateCustomerCommand()
            {
                ApplicationUserFk = userId
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

            product.Should().NotBeNull();
            product.Name.Should().Be(productCommand.Name);
            product.Image.Should().BeNull();
            product.Category.Should().Be(productCommand.Category);
            product.Price.Should().Be(productCommand.Price);
            product.Discount.Should().Be(productCommand.Discount);
            product.Quantity.Should().Be(productCommand.Quantity);
            product.RestaurateurId.Should().Be(productCommand.Restaurateur.Id);
        }
    }
}
