using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class RestaurateurConfiguration : IEntityTypeConfiguration<Restaurateur>
    {
        public void Configure(EntityTypeBuilder<Restaurateur> builder)
        {
            builder.HasOne(r => r.Customer)
                .WithOne()
                .HasForeignKey<Restaurateur>(c => c.CustomerId)
                .OnDelete(DeleteBehavior.ClientCascade);

            //builder.HasMany(r => r.RestaurantOrders)
            //    .WithOne(o => o.Restaurateur)
            //    .HasForeignKey(o => o.RestaurateurId)
            //    .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(r => r.RestaurantAddress)
                .WithOne(a => a.Restaurateur)
                .HasForeignKey<Address>(a => a.RestaurateurId)
                .OnDelete(DeleteBehavior.SetNull);

            //builder.HasMany(r => r.RestaurateurReviews)
            //    .WithOne(r => r.Restaurateur)
            //    .HasForeignKey(r => r.RestaurateurId)
            //    .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
