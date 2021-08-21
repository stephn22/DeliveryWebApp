using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(r => r.Title)
                .IsRequired();

            builder.Property(r => r.Rating)
                .IsRequired();

            builder.Property(r => r.Date)
                .HasColumnType("datetime2")
                .HasPrecision(0)
                .IsRequired();

            builder.Property(r => r.Text)
                .HasMaxLength(250)
                .IsRequired();

            builder.HasIndex(r => r.CustomerId)
                .IsUnique();

            builder.HasIndex(r => r.RestaurateurId)
                .IsUnique();

            builder.HasOne(r => r.Customer)
                .WithMany(c => c.Reviews)
                .HasForeignKey(r => r.CustomerId)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
