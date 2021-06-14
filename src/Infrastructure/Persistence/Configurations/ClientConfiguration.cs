﻿using System;
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
            builder.Property(typeof(ApplicationUser), PropertyName.User)
                .IsRequired();

            builder.Property(r => r.Address)
                .IsRequired();
        }
    }
}
