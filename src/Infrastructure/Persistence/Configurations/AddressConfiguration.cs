using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(c => c.AddressLine1)
                .IsRequired();

            builder.Property(c => c.Number)
                .IsRequired();

            builder.Property(c => c.City)
                .IsRequired();

            builder.Property(c => c.PostalCode)
                .IsRequired();

            builder.Property(c => c.Country)
                .IsRequired();

            builder.Property(a => a.Longitude)
                .HasPrecision(16, 3)
                .IsRequired();

            builder.Property(a => a.Latitude)
                .HasPrecision(16, 3)
                .IsRequired();

            builder.HasOne(a => a.Customer)
                .WithMany(c => c.Addresses)
                .HasForeignKey(c => c.Id)
                .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}
