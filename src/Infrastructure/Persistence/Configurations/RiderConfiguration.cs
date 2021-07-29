using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence.Configurations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class RiderConfiguration : IEntityTypeConfiguration<Rider>
    {
        public void Configure(EntityTypeBuilder<Rider> builder)
        {
            builder.HasKey(u => u.Id);

            //builder.HasOne(u => u.Customer)
            //    .WithOne()
            //    .HasForeignKey<Customer>(u => u.Id);

            builder.Property(r => r.DeliveryCredit)
                .HasPrecision(19, 4)
                .HasColumnType(PropertyName.Money)
                .IsRequired();

            //builder.HasMany(u => u.OpenOrders)
            //    .WithOne()
            //    .HasForeignKey(u => u.Id);
        }
    }
}
