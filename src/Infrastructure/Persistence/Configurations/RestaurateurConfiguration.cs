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
    public class RestaurateurConfiguration : IEntityTypeConfiguration<Restaurateur>
    {
        public void Configure(EntityTypeBuilder<Restaurateur> builder)
        {
            //builder.HasOne(u => u.Customer)
            //    .WithOne()
            //    .HasForeignKey<Customer>(u => u.Id);

            builder.HasMany(r => r.Reviews)
                .WithOne(r => r.Restaurateur)
                .HasForeignKey(r => r.RestaurateurId);

            builder.HasOne(u => u.Restaurant)
                .WithOne(c => c.Restaurateur)
                .HasForeignKey<Restaurant>(u => u.RestaurateurId);
        }
    }
}
