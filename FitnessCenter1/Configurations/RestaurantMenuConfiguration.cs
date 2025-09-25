using FitnessCenter1.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FitnessCenter1.Configurations
{
    public class RestaurantMenuConfiguration : IEntityTypeConfiguration<RestaurantMenu>
    {
        public void Configure(EntityTypeBuilder<RestaurantMenu> entity)
        {
            entity.HasKey(rm => rm.RestaurantMenuId);

            entity.Property(rm => rm.Name).IsRequired().HasMaxLength(100);
            entity.Property(rm => rm.Description).HasMaxLength(500);
            entity.Property(rm => rm.Price).HasColumnType("decimal(18,2)").IsRequired();
            entity.Property(rm => rm.Category).IsRequired().HasMaxLength(10);
            entity.Property(rm => rm.SpecialOfferPrice).HasColumnType("decimal(18,2)");

            entity.HasMany(rm => rm.RestaurantOrders)
                  .WithOne(ro => ro.RestaurantMenu)
                  .HasForeignKey(ro => ro.RestaurantMenuId);
        }
    }
}
