using FitnessCenter1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCenter1.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.HasKey(u => u.UserId);

            entity.Property(u => u.Name).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Surname).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Username).IsRequired().HasMaxLength(30);
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Email).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Gender).IsRequired().HasMaxLength(10);
            entity.Property(u => u.Money).HasColumnType("decimal(18,2)").HasDefaultValue(0);
            entity.Property(u => u.HasEnteredGymToday).HasDefaultValue(false);

            entity.HasMany(u => u.ParkingReservations).WithOne(pr => pr.User).HasForeignKey(pr => pr.UserId);
            entity.HasMany(u => u.RestaurantOrders).WithOne(ro => ro.User).HasForeignKey(ro => ro.UserId);
            entity.HasMany(u => u.ProgramUsages).WithOne(fsu => fsu.User).HasForeignKey(fsu => fsu.UserId);
            entity.HasMany(u => u.TrainingSessions).WithOne(ts => ts.User).HasForeignKey(ts => ts.UserId);
            entity.HasMany(u => u.Memberships).WithOne(m => m.User).HasForeignKey(m => m.UserId);
            entity.HasMany(u => u.Payments).WithOne(p => p.User).HasForeignKey(p => p.UserId);
            entity.HasMany(u => u.EquipmentUsages).WithOne(eu => eu.User).HasForeignKey(eu => eu.UserId);
            entity.HasMany(u => u.Notifications).WithOne(n => n.User).HasForeignKey(n => n.UserId);
        }
    }
}
