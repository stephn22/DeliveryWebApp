using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Customers.Commands
{
    using static Testing;

    public class CreateCustomerTest : TestBase
    {
        //[Test]
        //public void ShouldRequireMinimumFields()
        //{
        //    var command = new CreateCustomerCommand();

        //    FluentActions.Invoking(() =>
        //        SendAsync(command)).Should().Throw<ValidationException>();
        //}

        [Test]
        public async Task ShouldCreateCustomerAsync()
        {
            var userId = await RunAsDefaultUserAsync();

            var command = new CreateCustomerCommand
            {
                ApplicationUserFk = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com"
            };

            var customer = await SendAsync(command);

            customer.Should().NotBeNull();
            customer.Id.Should().NotBe(0);
            customer.ApplicationUserFk.Should().Be(command.ApplicationUserFk);
            customer.FirstName.Should().Be(command.FirstName);
            customer.LastName.Should().Be(command.LastName);
            customer.Addresses.Should().BeNullOrEmpty();
        }
    }
}
