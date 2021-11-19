using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.Property(a => a.PlaceName)
                .IsRequired();

            builder.Property(a => a.Longitude)
                .HasPrecision(18, 9)
                .IsRequired();

            builder.Property(a => a.Latitude)
                .HasPrecision(18, 9)
                .IsRequired();
        }
    }
}
