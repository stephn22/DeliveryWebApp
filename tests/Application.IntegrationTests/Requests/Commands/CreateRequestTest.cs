using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Common.Security;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Requests.Commands.CreateRequest;
using DeliveryWebApp.Domain.Constants;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Requests.Commands;

using static Testing;

public class CreateRequestTest : TestBase
{
    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateCustomerCommand();

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateRequestAsync()
    {
        // create customer first
        var userId = await RunAsDefaultUserAsync();

        var cmd = new CreateCustomerCommand
        {
            ApplicationUserFk = userId,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@gmail.com"
        };

        var customer = await SendAsync(cmd);

        // then create request
        var command = new CreateRequestCommand
        {
            CustomerId = customer.Id,
            Role = RoleName.Restaurateur,
            Status = RequestStatus.Idle
        };

        var item = await SendAsync(command);

        var request = await FindAsync<Request>(item.Id);

        request.Should().NotBeNull();
        request.Id.Should().BeGreaterThan(0);
        request.CustomerId.Should().Be(customer.Id);
        request.Status.Should().Be(command.Status);
        request.Role.Should().Be(command.Role);
        request.Customer.Should().BeNull();
    }
}