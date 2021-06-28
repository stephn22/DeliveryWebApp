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
            //builder.HasMany(u => u.Products)
            //    .WithOne()
            //    .HasForeignKey(u => u.Id);

            builder.Property(r => r.Name)
                .IsRequired();

            builder.Property(r => r.LogoUrl)
                .IsRequired();

            //builder.HasOne(r => r.Restaurateur)
            //    .WithOne()
            //    .HasForeignKey<Restaurateur>(r => r.Id)
            //    .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
