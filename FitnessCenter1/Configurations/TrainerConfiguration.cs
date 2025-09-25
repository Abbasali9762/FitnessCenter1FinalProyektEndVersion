using FitnessCenter1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCenter1.Configurations
{
    public class TrainerConfiguration : IEntityTypeConfiguration<Trainer>
    {
        public void Configure(EntityTypeBuilder<Trainer> entity)
        {
            entity.HasKey(t => t.TrainerId);

            entity.Property(t => t.Name).IsRequired().HasMaxLength(50);
            entity.Property(t => t.Surname).IsRequired().HasMaxLength(50);
            entity.Property(t => t.Specialization).HasMaxLength(100);
            entity.Property(t => t.HourlyRate).HasColumnType("decimal(18,2)").IsRequired();

            entity.HasMany(t => t.TrainingSessions)
                  .WithOne(ts => ts.Trainer)
                  .HasForeignKey(ts => ts.TrainerId);
        }
    }
}
