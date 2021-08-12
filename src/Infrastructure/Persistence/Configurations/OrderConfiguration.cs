using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence.Configurations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(u => u.TotalPrice)
                .HasPrecision(16, 3)
                .HasColumnType(PropertyName.Money)
                .IsRequired();

            builder.Property(o => o.Date)
                .HasColumnType("datetime2")
                .HasPrecision(0)
                .IsRequired();

            builder.Property(u => u.Status)
                .IsRequired();

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
