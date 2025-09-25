using FitnessCenter1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCenter1.Configurations
{
    public class TrainingSessionConfiguration : IEntityTypeConfiguration<TrainingSession>
    {
        public void Configure(EntityTypeBuilder<TrainingSession> entity)
        {
            entity.HasKey(ts => ts.TrainingSessionId);

            entity.Property(ts => ts.TotalPrice).HasColumnType("decimal(18,2)");

            entity.HasOne(ts => ts.User)
                  .WithMany(u => u.TrainingSessions)
                  .HasForeignKey(ts => ts.UserId);

            entity.HasOne(ts => ts.Trainer)
                  .WithMany(t => t.TrainingSessions)
                  .HasForeignKey(ts => ts.TrainerId);
        }
    }
}
