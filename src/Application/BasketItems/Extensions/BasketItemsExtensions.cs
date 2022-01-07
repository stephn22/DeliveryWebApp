using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.BasketItems.Extensions;

public static class BasketItemsExtensions
{
    public static async Task<Product> GetProduct(this BasketItem basketItem, IApplicationDbContext context)
    {
        return await context.Products.FindAsync(basketItem.ProductId);
    }
}