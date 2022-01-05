using DeliveryWebApp.Application.Common.Exceptions;
using DeliveryWebApp.Application.Orders.Commands.DeleteOrder;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.IntegrationTests.Orders.Commands
{
    using static Testing;

    public class DeleteOrderTest : TestBase
    {
        [Test]
        public async Task ShouldRequireMinimumFields()
        {
            var command = new DeleteOrderCommand();

            await FluentActions.Invoking(() =>
                SendAsync(command)).Should().ThrowAsync<ValidationException>();
        }
    }
}
