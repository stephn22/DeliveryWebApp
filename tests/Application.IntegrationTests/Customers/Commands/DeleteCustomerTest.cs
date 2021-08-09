using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Customers.Commands.CreateCustomer;
using DeliveryWebApp.Application.Customers.Commands.DeleteCustomer;
using DeliveryWebApp.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;

namespace DeliveryWebApp.Application.IntegrationTests.Customers.Commands
{
    using static Testing;

    public class DeleteCustomerTest : TestBase
    {
        //[Test]
        //public void ShouldRequireMinimumFields()
        //{
        //    var command = new DeleteCustomerCommand();

        //    FluentActions.Invoking(() =>
        //        SendAsync(command)).Should().Throw<ValidationException>();
        //}

        [Test]
        public async Task ShouldDeleteCustomerAsync()
        {
            var userId = await RunAsAdministratorAsync();

            var create = new CreateCustomerCommand
            {
                ApplicationUserFk = userId
            };

            var item = await SendAsync(create);

            await SendAsync(new DeleteCustomerCommand
            {
                Id = item.Id
            });

            var customer = await FindAsync<Customer>(item.Id);
            customer.Should().BeNull();
        }
    }
}
