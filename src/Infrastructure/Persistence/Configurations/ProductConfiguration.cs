using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired();

            builder.Property(p => p.Quantity)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasPrecision(16, 4)
                .IsRequired();

            builder.HasOne(p => p.Restaurateur)
                .WithMany(r => r.Products)
                .HasForeignKey(p => p.RestaurateurId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
