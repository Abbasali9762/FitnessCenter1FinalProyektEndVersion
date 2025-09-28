using Microsoft.EntityFrameworkCore;
using FitnessCenter1.Entities;
using FitnessCenter1.Configurations;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FitnessCenter1.Context
{
    public class FitnessCenterDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LAPTOP-1QHMBOUE;Initial Catalog=dbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbbb;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True",
                    options => options.EnableRetryOnFailure()
                );
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ParkingArea> ParkingAreas { get; set; }
        public DbSet<ParkingReservation> ParkingReservations { get; set; }
        public DbSet<RestaurantMenu> RestaurantMenus { get; set; }
        public DbSet<RestaurantOrder> RestaurantOrders { get; set; }
        public DbSet<FitnessProgram> FitnessPrograms { get; set; }
        public DbSet<ProgramUsage> ProgramUsages { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<EquipmentUsage> EquipmentUsages { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FitnessCenterDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}