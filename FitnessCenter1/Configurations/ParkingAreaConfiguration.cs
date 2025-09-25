using FitnessCenter1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCenter1.Configurations
{
    public class ParkingAreaConfiguration : IEntityTypeConfiguration<ParkingArea>
    {
        public void Configure(EntityTypeBuilder<ParkingArea> entity)
        {
            entity.HasKey(pa => pa.ParkingAreaId);

            entity.Property(pa => pa.Name).IsRequired().HasMaxLength(50);
            entity.Property(pa => pa.TotalSpots).IsRequired();
            entity.Property(pa => pa.AvailableSpots).IsRequired();

            entity.HasMany(pa => pa.ParkingReservations)
                  .WithOne(pr => pr.ParkingArea)
                  .HasForeignKey(pr => pr.ParkingAreaId);
        }
    }
}
