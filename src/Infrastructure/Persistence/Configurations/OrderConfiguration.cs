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
            //builder.HasMany(u => u.Products)
            //    .WithOne()
            //    .HasForeignKey(u => u.Id)
            //    .OnDelete(DeleteBehavior.NoAction);

            builder.Property(u => u.TotalPrice)
                .HasPrecision(19, 4)
                .HasColumnType(PropertyName.Money)
                .IsRequired();

            builder.Property(o => o.Date)
                .IsRequired();

            builder.Property(u => u.Status)
                .IsRequired();

            builder.HasOne(u => u.Customer)
                .WithOne()
                .HasForeignKey<Customer>(c => c.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
