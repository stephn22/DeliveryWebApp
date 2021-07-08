using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Persistence.Configurations.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .IsRequired();

            builder.Property(p => p.Quantity)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasPrecision(19, 4)
                .HasColumnType(PropertyName.Money)
                .IsRequired();
        }
    }
}
