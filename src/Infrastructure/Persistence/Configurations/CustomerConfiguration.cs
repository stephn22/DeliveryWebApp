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
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(u => u.ApplicationUserFk)
                .IsRequired();

            //builder.HasOne(c => c.Basket)
            //    .WithOne(b => b.Customer)
            //    .HasForeignKey<Basket>(b => b.CustomerId);

            //builder.HasMany(c => c.Addresses)
            //    .WithOne()
            //    .HasForeignKey(c => c.Id);

            //builder.HasMany(r => r.Reviews)
            //    .WithOne(c => c.Customer)
            //    .HasForeignKey(c => c.CustomerId);

            //builder.HasMany(r => r.Orders)
            //    .WithOne(c => c.Customer)
            //    .HasForeignKey(r => r.CustomerId);
        }
    }
}
