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
        DbSet<Order> Orders { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Restaurant> Restaurants { get; set; }
        DbSet<Restaurateur> Restaurateurs { get; set; }
        DbSet<Rider> Riders { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
