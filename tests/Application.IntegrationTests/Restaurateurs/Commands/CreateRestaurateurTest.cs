using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Restaurateurs.Commands.CreateRestaurateur;
using FluentAssertions;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Restaurateurs.Commands;

using static Testing;

public class CreateRestaurateurTest : TestBase
{

    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateRestaurateurCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateRestaurateurAsync()
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

        var restaurateur = await SendAsync(command);

        restaurateur.Should().NotBeNull();
        restaurateur.Id.Should().NotBe(0);
        restaurateur.RestaurantName.Should().BeNull();
        restaurateur.RestaurantAddress.Should().BeNull();
        restaurateur.RestaurantCategory.Should().BeNull();
        restaurateur.CustomerId.Should().Be(customer.Id);
    }
}