using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Products.Commands.CreateProduct;
using DeliveryWebApp.Application.Products.Commands.DeleteProduct;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Products.Commands;

using static Testing;
public class DeleteProductTest : TestBase
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new DeleteProductCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

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
            Category = ProductCategory.Pizza,
            Price = 5.50M,
            Discount = 12,
            Quantity = 21,
            RestaurateurId = restaurateur.Id
        };

        var item = await SendAsync(productCommand);

        await SendAsync(new DeleteProductCommand
        {
            Id = item.Id
        });

        var product = await FindAsync<Product>(item.Id);
        product.Should().BeNull();
    }
}