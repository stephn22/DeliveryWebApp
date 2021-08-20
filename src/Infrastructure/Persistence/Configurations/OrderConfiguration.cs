using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(u => u.TotalPrice)
                .HasPrecision(16, 4)
                .IsRequired();

            builder.Property(o => o.Date)
                .HasColumnType("datetime2")
                .HasPrecision(0)
                .IsRequired();

            builder.Property(o => o.DeliveryDate)
                .HasColumnType("datetime2")
                .HasPrecision(0);

            builder.Property(u => u.Status)
                .IsRequired();

            builder.Property(o => o.AddressId)
                .IsRequired();

            builder.HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
