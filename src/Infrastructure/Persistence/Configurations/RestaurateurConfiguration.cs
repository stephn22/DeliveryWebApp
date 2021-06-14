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
    public class RestaurateurConfiguration : IEntityTypeConfiguration<Restaurateur>
    {
        public void Configure(EntityTypeBuilder<Restaurateur> builder)
        {
            builder.Property(o => o.Restaurant)
                .IsRequired();
        }
    }
}
