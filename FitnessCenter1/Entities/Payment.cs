using System.ComponentModel.DataAnnotations;
namespace FitnessCenter1.Entities
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? PaymentType { get; set; }

        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

        [MaxLength(20)]
        public string? Status { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public virtual User? User { get; set; }
    }
}