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
    public class RequestConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.HasKey(c => c.Id);

            builder.HasOne(c => c.Client)
                .WithOne()
                .HasForeignKey<Client>(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(c => c.Role)
                .IsRequired();
        }
    }
}
