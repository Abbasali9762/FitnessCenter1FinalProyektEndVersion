using FitnessCenter1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCenter1.Configurations
{
    public class RestaurantOrderConfiguration : IEntityTypeConfiguration<RestaurantOrder>
    {
        public void Configure(EntityTypeBuilder<RestaurantOrder> entity)
        {
            entity.HasKey(ro => ro.RestaurantOrderId);

            entity.Property(ro => ro.TotalPrice).HasColumnType("decimal(18,2)");

            entity.HasOne(ro => ro.User)
                  .WithMany(u => u.RestaurantOrders)
                  .HasForeignKey(ro => ro.UserId);

            entity.HasOne(ro => ro.RestaurantMenu)
                  .WithMany(rm => rm.RestaurantOrders)
                  .HasForeignKey(ro => ro.RestaurantMenuId);
        }
    }
}
