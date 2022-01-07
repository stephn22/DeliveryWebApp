using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence.Configurations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.Property(u => u.TotalPrice)
            .HasColumnType(ColumnType.Money)
            .IsRequired();

        builder.Property(o => o.Date)
            .HasColumnType(ColumnType.DateTime)
            .HasPrecision(0)
            .IsRequired();

        builder.Property(o => o.DeliveryDate)
            .HasColumnType(ColumnType.DateTime)
            .HasPrecision(0);

        builder.Property(u => u.Status)
            .IsRequired();

        builder.Property(o => o.DeliveryAddressId)
            .IsRequired();

        //builder.HasOne(o => o.Customer)
        //    .WithMany(c => c.Orders)
        //    .HasForeignKey(o => o.CustomerId)
        //    .OnDelete(DeleteBehavior.ClientCascade);
    }
}