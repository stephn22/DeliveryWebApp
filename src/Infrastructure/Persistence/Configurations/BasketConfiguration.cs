using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryWebApp.Application.TodoLists.Queries.GetTodos;
using DeliveryWebApp.Domain.Entities;
using DeliveryWebApp.Infrastructure.Identity;
using DeliveryWebApp.Infrastructure.Persistence.Configurations.Constants;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class BasketConfiguration : IEntityTypeConfiguration<Basket>
    {
        public void Configure(EntityTypeBuilder<Basket> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasOne(u => u.Client)
                .WithOne()
                .HasForeignKey<Client>(u => u.ApplicationUserFk)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            builder.HasMany(c => c.Products)
                .WithOne()
                .HasForeignKey(c => c.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(u => u.TotalPrice)
                .IsRequired();
        }
    }
}
