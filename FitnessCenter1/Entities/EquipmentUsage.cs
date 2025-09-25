using System.ComponentModel.DataAnnotations;
namespace FitnessCenter1.Entities
{
    public class EquipmentUsage
    {
        public int EquipmentUsageId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int EquipmentId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int DurationMinutes => EndTime.HasValue ?
            (int)(EndTime.Value - StartTime).TotalMinutes : 0;

        public virtual User? User { get; set; }
        public virtual Equipment? Equipment { get; set; }
    }
}