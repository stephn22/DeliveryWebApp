using DeliveryWebApp.Domain.Entities;
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
                .IsRequired();

            builder.Property(o => o.Date)
                .IsRequired();

            builder.Property(u => u.Status)
                .IsRequired();
        }
    }
}
