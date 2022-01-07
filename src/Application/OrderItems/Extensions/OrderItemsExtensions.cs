using DeliveryWebApp.Application.Common.Interfaces;
using DeliveryWebApp.Domain.Entities;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.OrderItems.Extensions;

public static class OrderItemsExtensions
{
    public static async Task<Product> GetProduct(this OrderItem orderItem, IApplicationDbContext context)
    {
        return await context.Products.FindAsync(orderItem.ProductId);
    }
}