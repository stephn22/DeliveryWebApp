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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Client)
                .IsRequired();

            builder.Property(o => o.Products)
                .IsRequired();

            builder.Property(o => o.Date)
                .IsRequired();

            builder.Property(o => o.Restaurant)
                .IsRequired();
        }
    }
}
