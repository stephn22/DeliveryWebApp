using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.Products)
                .WithOne()
                .HasForeignKey(u => u.Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(u => u.Restaurant)
                .WithOne()
                .HasForeignKey<Restaurant>(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(u => u.TotalPrice)
                .IsRequired();

            builder.Property(o => o.Date)
                .IsRequired();

            builder.Property(u => u.Status)
                .IsRequired();
        }
    }
}
