using System.ComponentModel.DataAnnotations;

namespace FitnessCenter1.Entities
{
    public class Notification
    {
        public int NotificationId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(1000)]
        public string? Message { get; set; }

        public DateTime CreatedDate { get; set; }
        public bool IsRead { get; set; }

        [MaxLength(50)]
        public string? Type { get; set; }

        public virtual User? User { get; set; }
    }
}