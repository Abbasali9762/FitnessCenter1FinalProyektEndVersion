using FitnessCenter1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCenter1.Configurations
{
    public class ParkingReservationConfiguration : IEntityTypeConfiguration<ParkingReservation>
    {
        public void Configure(EntityTypeBuilder<ParkingReservation> entity)
        {
            entity.HasKey(pr => pr.ParkingReservationId);

            entity.HasOne(pr => pr.User)
                  .WithMany(u => u.ParkingReservations)
                  .HasForeignKey(pr => pr.UserId);

            entity.HasOne(pr => pr.ParkingArea)
                  .WithMany(pa => pa.ParkingReservations)
                  .HasForeignKey(pr => pr.ParkingAreaId);
        }
    }
}
