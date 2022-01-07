using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Products.Commands.UpdateProducts;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Domain.Constants;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Products.Commands;

using static Testing;

public class UpdateProductTest : TestBase
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new UpdateProductCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

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
            Category = ProductCategory.Pizza,
            Price = 5.50M,
            Discount = 12,
            Quantity = 21,
            RestaurateurId = restaurateur.Id
        };

        var product = await SendAsync(productCommand);

        var updateCommand = new UpdateProductCommand
        {
            Id = product.Id,
            Image = product.Image,
            Name = "French Fries",
            Category = ProductCategory.Snacks,
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