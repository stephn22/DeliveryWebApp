using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class RestaurateurConfiguration : IEntityTypeConfiguration<Restaurateur>
    {
        public void Configure(EntityTypeBuilder<Restaurateur> builder)
        {
            builder.ToTable("Restaurateurs");

            builder.HasOne(r => r.RestaurantAddress)
                .WithOne(r => r.Restaurateur)
                .HasForeignKey<Address>(a => a.RestaurateurId);
        }
    }
}
