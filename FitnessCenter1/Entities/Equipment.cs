using System.ComponentModel.DataAnnotations;

namespace FitnessCenter1.Entities
{
    public class Equipment
    {
        public int EquipmentId { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        public int AvailableQuantity { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Category { get; set; }

        public DateTime PurchaseDate { get; set; }
        public DateTime? MaintenanceDate { get; set; }

        public bool NeedsMaintenance => MaintenanceDate.HasValue && MaintenanceDate <= DateTime.Now;
        public bool IsAvailable => AvailableQuantity > 0;
    }
}