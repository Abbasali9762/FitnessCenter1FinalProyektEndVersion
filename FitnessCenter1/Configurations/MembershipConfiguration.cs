using FitnessCenter1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCenter1.Configurations
{
    public class MembershipConfiguration : IEntityTypeConfiguration<Membership>
    {
        public void Configure(EntityTypeBuilder<Membership> entity)
        {
            entity.HasKey(m => m.MembershipId);

            entity.Property(m => m.Type).IsRequired().HasMaxLength(50);
            entity.Property(m => m.Price).HasColumnType("decimal(18,2)").IsRequired();

            entity.HasOne(m => m.User)
                  .WithMany(u => u.Memberships)
                  .HasForeignKey(m => m.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
