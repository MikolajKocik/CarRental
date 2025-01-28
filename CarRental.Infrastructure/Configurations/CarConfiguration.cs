using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Infrastructure.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<CarRental.Domain.Entities.Car>
    {
        public void Configure(EntityTypeBuilder<CarRental.Domain.Entities.Car> builder) 
        { 
            builder.Property(c => c.PricePerDay)
            .HasColumnType("decimal(18, 2)");
        }
    }
}
