using FitnessCenter1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCenter1.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> entity)
        {
            entity.HasKey(n => n.NotificationId);

            entity.Property(n => n.Title).IsRequired().HasMaxLength(200);
            entity.Property(n => n.Message).HasMaxLength(1000);
            entity.Property(n => n.Type).HasMaxLength(50).HasDefaultValue("Info");
            entity.Property(n => n.IsRead).HasDefaultValue(false);
            entity.Property(n => n.CreatedDate).HasDefaultValueSql("GETDATE()");

            entity.HasOne(n => n.User)
                  .WithMany(u => u.Notifications)
                  .HasForeignKey(n => n.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
