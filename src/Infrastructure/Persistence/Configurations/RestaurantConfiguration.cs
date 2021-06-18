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

            builder.HasOne(u => u.Restaurateur)
                .WithOne()
                .HasForeignKey<Restaurateur>(u => u.ApplicationUserFk)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(u => u.Products)
                .WithOne()
                .HasForeignKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasOne(u => u.Address)
                .WithOne()
                .HasForeignKey<Address>(u => u.Id)
                .IsRequired();

            builder.Property(r => r.Name)
                .IsRequired();

            builder.Property(r => r.LogoUrl)
                .IsRequired();

            builder.Property(u => u.Name)
                .IsRequired();
        }
    }
}
