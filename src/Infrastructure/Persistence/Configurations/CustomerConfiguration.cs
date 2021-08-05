using DeliveryWebApp.Domain.Entities;
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

            builder.HasMany(c => c.Addresses)
                .WithOne(c => c.Customer)
                .HasForeignKey(c => c.CustomerId);

            //builder.HasMany(r => r.Reviews)
            //    .WithOne(c => c.Customer)
            //    .HasForeignKey(c => c.CustomerId);

            //builder.HasMany(r => r.Orders)
            //    .WithOne(c => c.Customer)
            //    .HasForeignKey(r => r.CustomerId);
        }
    }
}
