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
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.HasKey(r => r.Id);

            builder.HasOne(r => r.Client)
                .WithOne()
                .HasForeignKey<Client>(c => c.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(r => r.Restaurant)
                .WithOne()
                .HasForeignKey<Restaurant>(r => r.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Restaurateur)
                .WithOne()
                .HasForeignKey<Restaurateur>(r => r.Id)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(r => r.Title)
                .IsRequired();

            builder.Property(r => r.Grade)
                .IsRequired();

            builder.Property(r => r.Text)
                .HasMaxLength(250)
                .IsRequired();
        }
    }
}
