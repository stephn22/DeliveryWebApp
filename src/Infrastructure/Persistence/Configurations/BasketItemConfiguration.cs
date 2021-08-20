using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class BasketItemConfiguration : IEntityTypeConfiguration<BasketItem>
    {
        public void Configure(EntityTypeBuilder<BasketItem> builder)
        {
            builder.Property(b => b.Quantity)
                .IsRequired();

            builder.HasOne(b => b.Basket)
                .WithMany(b => b.BasketItems)
                .HasForeignKey(b => b.BasketId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
