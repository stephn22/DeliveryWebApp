using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence.Configurations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations;

public class RiderConfiguration : IEntityTypeConfiguration<Rider>
{
    public void Configure(EntityTypeBuilder<Rider> builder)
    {
        builder.Property(r => r.DeliveryCredit)
            .HasColumnType(ColumnType.Money)
            .IsRequired();

        builder.Property(r => r.TotalCredit)
            .HasColumnType(ColumnType.Money)
            .IsRequired();

        //builder.HasOne(r => r.Customer)
        //    .WithOne()
        //    .HasForeignKey<Rider>(c => c.CustomerId)
        //    .OnDelete(DeleteBehavior.ClientCascade);
    }
}