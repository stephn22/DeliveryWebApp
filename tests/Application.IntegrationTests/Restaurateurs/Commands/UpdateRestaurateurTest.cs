using DeliveryWebApp.Application.Restaurateurs.Commands.UpdateRestaurateur;
using DeliveryWebApp.Domain.Entities;
using NUnit.Framework;

namespace DeliveryWebApp.Application.IntegrationTests.Restaurateurs.Commands
{
    using static Testing;
    public class UpdateRestaurateurTest : TestBase
    {
        [Test]
        public void ShouldRequireValidRestaurateurId()
        {
            var command = new UpdateRestaurateurCommand
            {
                Id = 5,
                Restaurant = new Restaurant
                {
                    // TODO: complete
                }
            };
        }
    }
}
