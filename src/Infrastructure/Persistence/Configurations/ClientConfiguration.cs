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
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.Property(u => u.ApplicationUserFk)
                .IsRequired();

            //builder.HasOne(c => c.Basket)
            //    .WithOne(b => b.Client)
            //    .HasForeignKey<Basket>(b => b.ClientId);

            //builder.HasMany(c => c.Addresses)
            //    .WithOne()
            //    .HasForeignKey(c => c.Id);

            //builder.HasMany(r => r.Reviews)
            //    .WithOne(c => c.Client)
            //    .HasForeignKey(c => c.ClientId);

            //builder.HasMany(r => r.Orders)
            //    .WithOne(c => c.Client)
            //    .HasForeignKey(r => r.ClientId);
        }
    }
}
