using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Infrastructure.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<CarRental.Domain.Entities.Reservation>
    {
        public void Configure(EntityTypeBuilder<CarRental.Domain.Entities.Reservation> builder)
        {
            // Relation 1-* between Car and Reservations

            builder.HasOne(r => r.Car)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.CarId)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
