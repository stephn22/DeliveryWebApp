using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Riders.Commands.CreateRider;
using DeliveryWebApp.Application.Riders.Commands.DeleteRider;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeliveryWebApp.Application.IntegrationTests.Riders.Commands
{
    using static Testing;

    public class DeleteRiderTest : TestBase
    {
        [Test]
        public async Task ShouldDeteleRiderAsync()
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

            var command = new CreateRiderCommand
            {
                Customer = customer,
                DeliveryCredit = 12.52M
            };

            var item = await SendAsync(command);

            await SendAsync(new DeleteRiderCommand
            {
                Rider = item
            });

            var rider = await FindAsync<Rider>(item.Id);
            rider.Should().BeNull();
            customer.Should().NotBeNull();
        }
    }
}
