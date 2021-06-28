using DeliveryWebApp.Domain.Entities;
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
                .IsRequired();
        }
    }
}
