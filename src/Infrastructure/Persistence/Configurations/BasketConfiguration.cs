using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence.Configurations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            //builder.HasMany(b => b.Products)
            //    .WithOne()
            //    .HasForeignKey(e => e.Id)
            //    .OnDelete(DeleteBehavior.NoAction);

            builder.Property(u => u.TotalPrice)
                .HasPrecision(19, 4)
                .HasColumnType(PropertyName.Money)
                .IsRequired();
        }
    }
}
