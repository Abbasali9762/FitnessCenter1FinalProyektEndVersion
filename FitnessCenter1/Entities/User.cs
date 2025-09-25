using System.ComponentModel.DataAnnotations;
namespace FitnessCenter1.Entities
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Surname { get; set; }

        [Required]
        [MaxLength(30)]
        public string? Username { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MaxLength(10)]
        public string? Gender { get; set; }

        public bool IsCar { get; set; }
        public decimal Money { get; set; }
        public bool HasEnteredGymToday { get; set; }
        public DateTime? LastGymEntryDate { get; set; }
        public string? OTP { get; set; }
        public DateTime? OTPExpiry { get; set; }

        public virtual ICollection<ParkingReservation>? ParkingReservations { get; set; }
        public virtual ICollection<RestaurantOrder>? RestaurantOrders { get; set; }
        public virtual ICollection<ProgramUsage>? ProgramUsages { get; set; }
        public virtual ICollection<TrainingSession>? TrainingSessions { get; set; }
        public virtual ICollection<Membership>? Memberships { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
        public virtual ICollection<EquipmentUsage>? EquipmentUsages { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }
    }
}