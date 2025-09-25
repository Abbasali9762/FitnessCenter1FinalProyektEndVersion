using FitnessCenter1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCenter1.Configurations
{
    public class EquipmentUsageConfiguration : IEntityTypeConfiguration<EquipmentUsage>
    {
        public void Configure(EntityTypeBuilder<EquipmentUsage> entity)
        {
            entity.HasKey(eu => eu.EquipmentUsageId);

            entity.HasOne(eu => eu.User)
                  .WithMany(u => u.EquipmentUsages)
                  .HasForeignKey(eu => eu.UserId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(eu => eu.Equipment)
                  .WithMany()
                  .HasForeignKey(eu => eu.EquipmentId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
