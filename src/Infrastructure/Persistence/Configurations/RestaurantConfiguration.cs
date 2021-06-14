using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(r => r.Address)
                .IsRequired();

            builder.Property(r => r.Name)
                .IsRequired();

            builder.Property(r => r.LogoUrl)
                .IsRequired();

            builder.Property(r => r.Address)
                .IsRequired();

        }
    }
}
