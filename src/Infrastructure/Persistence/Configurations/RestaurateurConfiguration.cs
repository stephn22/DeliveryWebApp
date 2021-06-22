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
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.Client)
                .WithOne()
                .HasForeignKey<Client>(u => u.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(u => u.Restaurant)
                .WithOne()
                .HasForeignKey<Restaurant>(u => u.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
