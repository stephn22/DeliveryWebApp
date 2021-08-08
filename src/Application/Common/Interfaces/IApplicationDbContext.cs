using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace DeliveryWebApp.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Address> Addresses { get; set; }
        DbSet<Basket> Baskets { get; set; }
        DbSet<BasketItem> BasketItems { get; set; }
        DbSet<Order> Orders { get; set; }
        DbSet<OrderItem> OrderItems { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<Restaurateur> Restaurateurs { get; set; }
        DbSet<Rider> Riders { get; set; }
        DbSet<Request> Requests { get; set; }
        DbSet<Review> Reviews { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
