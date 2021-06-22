using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.Client)
                .WithOne()
                .HasForeignKey<Client>(u => u.Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.Products)
                .WithOne()
                .HasForeignKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

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
