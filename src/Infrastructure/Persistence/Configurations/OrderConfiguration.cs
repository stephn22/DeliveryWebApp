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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(u => u.Client)
                .WithOne()
                .HasForeignKey<Client>(u => u.ApplicationUserFk)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(u => u.Products)
                .WithOne()
                .HasForeignKey(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.Property(u => u.TotalPrice)
                .IsRequired();

            builder.Property(o => o.Date)
                .IsRequired();

            builder.Property(o => o.Restaurant)
                .IsRequired();
        }
    }
}
