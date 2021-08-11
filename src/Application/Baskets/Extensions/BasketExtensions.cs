using DeliveryWebApp.Application.BasketItems.Extensions;
using DeliveryWebApp.Application.BasketItems.Queries;
using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using MediatR;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Baskets.Extensions
{
    public static class BasketExtensions
    {
        public static async Task<decimal> GetBasketTotalPrice(this Basket basket, IMediator mediator, IApplicationDbContext context)
        {
            var tot = 0.00M;

            basket.BasketItems = await mediator.Send(new GetBasketItemsQuery
            {
                Basket = basket
            });

            if (basket.BasketItems == null)
            {
                return tot;
            }

            foreach (var item in basket.BasketItems)
            {
                var product = await item.GetProduct(context);

                tot += (product.ApplyDiscount() * item.Quantity);
            }

            return tot;
        }
    }
}
