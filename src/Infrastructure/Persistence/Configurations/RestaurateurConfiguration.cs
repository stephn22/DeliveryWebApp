using DeliveryWebApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryWebApp.Infrastructure.Persistence.Configurations
{
    public class RestaurateurConfiguration : IEntityTypeConfiguration<Restaurateur>
    {
        public void Configure(EntityTypeBuilder<Restaurateur> builder)
        {
            //builder.HasOne(r => r.Customer)
            //    .WithOne()
            //    .HasForeignKey<Customer>(c => c.Id);

            builder.ToTable("Restaurateurs");
        }
    }
}
